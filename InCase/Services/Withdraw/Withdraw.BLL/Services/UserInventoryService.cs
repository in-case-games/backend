using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.BLL.Models;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly ApplicationDbContext _context;

        public UserInventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserInventoryResponse>> Get(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.UserInventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<List<UserInventoryResponse>> Get(Guid userId, int count)
        {
            if(!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserInventory> inventories = await _context.UserInventories
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .OrderByDescending(ui => ui.Date)
                .Take(count)
                .ToListAsync();

            return inventories.ToResponse();
        }

        public async Task<UserInventoryResponse> GetById(Guid id)
        {
            UserInventory inventory = await _context.UserInventories
                .AsNoTracking()
                .FirstOrDefaultAsync(ui => ui.Id == id) ?? 
                throw new NotFoundException("Инвентарь не найден");

            return inventory.ToResponse();
        }

        public Task<UserInventoryResponse> Exchange(Guid id, Guid itemId, Guid userId)
        {
            throw new NotImplementedException();

        }
        public Task<decimal> Sell(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> SellLast(Guid itemId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
