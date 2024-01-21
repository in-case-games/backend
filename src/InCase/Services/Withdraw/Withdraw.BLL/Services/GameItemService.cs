using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services;

public class GameItemService(ApplicationDbContext context) : IGameItemService
{
    public async Task<GameItem?> GetAsync(Guid id, CancellationToken cancellation = default) => 
        await context.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(gi => gi.Id == id, cancellation);

    public async Task CreateAsync(GameItemTemplate template, CancellationToken cancellation = default)
    {
        var item = new GameItem
        {
            Id = template.Id,
            Cost = template.Cost,
            IdForMarket = template.IdForMarket
        };

        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == template.GameName, cancellation) ?? 
            throw new NotFoundException("Игра не найдена");

        item.GameId = game.Id;

        await context.Items.AddAsync(item, cancellation);
        await context.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(GameItemTemplate template, CancellationToken cancellation = default)
    {
        var item = await context.Items
            .FirstOrDefaultAsync(gi => gi.Id == template.Id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        var game = await context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == template.GameName, cancellation) ??
            throw new NotFoundException("Игра не найдена");

        if (template.IdForMarket is not null) item.IdForMarket = template.IdForMarket;

        item.GameId = game.Id;
        item.Cost = template.Cost;

        await context.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var item = await context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
            throw new NotFoundException("Предмет не найден");

        context.Items.Remove(item);
        await context.SaveChangesAsync(cancellation);
    }
}