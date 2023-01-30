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
        public async Task<UserInventory> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserInventory? userInventory = await context
                .UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            return userInventory ?? throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<UserInventory>> GetAll(Guid userId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.UserInventory.ToListAsync();
        }
        public async Task<bool> Create(UserInventory userInventory)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            await context.UserInventory.AddAsync(userInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserInventory userInventory)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

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
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

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
