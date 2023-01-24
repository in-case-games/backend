using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserInventoryRepository : IUserInventoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserInventoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserInventory> GetUserInventory(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserInventory? userInventory = await _context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            return userInventory ?? throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<UserInventory>> GetAllUserInventories(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.UserInventory.ToListAsync();
        }
        public async Task<bool> CreateUserInventory(UserInventory userInventory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            User? searchUser = await _context
                .User
                .FirstOrDefaultAsync(x => x.Id == userInventory.UserId);

            GameItem? searchItem = await _context
                .GameItem
                .FirstOrDefaultAsync(x => x.Id == userInventory.GameItemId);
            
            if(searchUser is null) throw new Exception("There is no such user, " +
                "review what data comes from the api");
            if (searchItem is null) throw new Exception("There is no such game item, " +
                "review what data comes from the api");

            await _context.UserInventory.AddAsync(userInventory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserInventory(UserInventory userInventory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserInventory? searchUserInventory = await _context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == userInventory.Id);

            if(searchUserInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            _context.Entry(searchUserInventory).CurrentValues.SetValues(userInventory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserInventory(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserInventory? searchUserInventory = await _context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            _context.UserInventory.Remove(searchUserInventory);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
