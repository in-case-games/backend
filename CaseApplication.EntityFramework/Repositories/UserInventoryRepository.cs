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
        public async Task<UserInventory?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UserInventory>> GetAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserInventory
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> Create(UserInventory userInventory)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            await context.UserInventory.AddAsync(userInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserInventory userInventory)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? searchUserInventory = await context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == userInventory.Id);

            if(searchUserInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            context.Entry(searchUserInventory).CurrentValues.SetValues(userInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? searchUserInventory = await context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            context.UserInventory.Remove(searchUserInventory);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
