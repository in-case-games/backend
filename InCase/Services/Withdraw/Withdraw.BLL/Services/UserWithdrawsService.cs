using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class UserWithdrawsService : IUserWithdrawsService
    {
        private readonly ApplicationDbContext _context;

        public UserWithdrawsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserHistoryWithdrawResponse> GetAsync(Guid id)
        {
            UserHistoryWithdraw withdraw = await _context.HistoryWithdraws
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhw => uhw.Id == id) ?? 
                throw new NotFoundException("История вывода не найдена");

            return withdraw.ToResponse();
        }

        public async Task<List<UserHistoryWithdrawResponse>> GetAsync(Guid userId, int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserHistoryWithdraw> withdraws = await _context.HistoryWithdraws
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .Where(uhw => uhw.UserId == userId)
                .OrderByDescending(uhw => uhw.Date)
                .Take(count)
                .ToListAsync();

            return withdraws.ToResponse();
        }

        public async Task<List<UserHistoryWithdrawResponse>> GetAsync(int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserHistoryWithdraw> withdraws = await _context.HistoryWithdraws
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .OrderByDescending(uhw => uhw.Date)
                .Take(count)
                .ToListAsync();

            return withdraws.ToResponse();
        }

        public async Task<UserInventoryResponse> TransferAsync(Guid id, Guid userId)
        {
            UserHistoryWithdraw withdraw = await _context.HistoryWithdraws
                .Include(uhw => uhw.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhw => uhw.Id == id && uhw.UserId == userId) ??
                throw new NotFoundException("История вывода не найдена");

            if (withdraw.Status?.Name is null || withdraw.Status.Name != "cancel")
                throw new ConflictException("Ваш предмет выводится");

            UserInventory inventory = new()
            {
                Date = withdraw.Date,
                FixedCost = withdraw.FixedCost,
                ItemId = withdraw.ItemId,
                UserId = userId
            };

            _context.HistoryWithdraws.Remove(withdraw);
            await _context.UserInventories.AddAsync(inventory);
            await _context.SaveChangesAsync();

            return inventory.ToResponse();
        }
    }
}
