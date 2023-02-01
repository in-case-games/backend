using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class GameItemRepository : IGameItemRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public GameItemRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GameItem?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameItem.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<GameItem>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameItem.ToListAsync();
        }

        public async Task<bool> Create(GameItem item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            
            item.Id = Guid.NewGuid();

            await context.GameItem.AddAsync(item);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(GameItem item)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? searchItem = await context.GameItem.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (searchItem is null) throw new Exception("There is no such item in the database, " +
                "review what data comes from the api");

            context.Entry(searchItem).CurrentValues.SetValues(item);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameItem? searchItem = await context.GameItem.FirstOrDefaultAsync(x => x.Id == id);

            if (searchItem is null) throw new Exception("There is no such item in the database, " +
                "review what data comes from the api");

            context.GameItem.Remove(searchItem);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
