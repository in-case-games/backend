using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.MassTransit;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services;

public class UserWithdrawsService(
    ApplicationDbContext context, 
    BasePublisher publisher, 
    ILogger<UserWithdrawsService> logger) : IUserWithdrawsService
{
    public async Task<UserHistoryWithdrawResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var withdraw = await context.Withdraws
            .Include(uhw => uhw.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(uhw => uhw.Id == id, cancellation) ?? 
            throw new NotFoundException("История вывода не найдена");

        return withdraw.ToResponse();
    }

    public async Task<List<UserHistoryWithdrawResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var withdraws = await context.Withdraws
            .Include(uhw => uhw.Status)
            .AsNoTracking()
            .Where(uhw => uhw.UserId == userId)
            .OrderByDescending(uhw => uhw.Date)
            .Take(count)
            .ToListAsync(cancellation);

        return withdraws.ToResponse();
    }

    public async Task<List<UserHistoryWithdrawResponse>> GetAsync(int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000) 
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

        var withdraws = await context.Withdraws
            .Include(uhw => uhw.Status)
            .AsNoTracking()
            .OrderByDescending(uhw => uhw.Date)
            .Take(count)
            .ToListAsync(cancellation);

        return withdraws.ToResponse();
    }

    public async Task<UserInventoryResponse> TransferAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        var withdraw = await context.Withdraws
            .Include(uhw => uhw.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(uhw => uhw.Id == id && uhw.UserId == userId, cancellation) ??
            throw new NotFoundException("История вывода не найдена");

        if (withdraw.Status?.Name is "blocked") throw new ConflictException("Предмет заблокирован, обратитесь к админу");
        if (withdraw.Status?.Name is not "cancel") throw new ConflictException("Ваш предмет выводится");

        var inventory = new UserInventory
        {
            Date = withdraw.Date,
            FixedCost = withdraw.FixedCost,
            ItemId = withdraw.ItemId,
            UserId = userId
        };

        context.Withdraws.Remove(withdraw);
        await context.Inventories.AddAsync(inventory, cancellation);
        await context.SaveChangesAsync(cancellation);

        await publisher.SendAsync(new SiteStatisticsAdminTemplate
        {
            FundsUsersInventories = inventory.FixedCost
        }, cancellation);

        logger.LogInformation($"Items successfully transferred. UserId: {userId}. UserHistoryWithdrawId: {id}");

        return inventory.ToResponse();
    }
}