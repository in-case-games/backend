using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class CaseInventoryRepository : ICaseInventoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CaseInventoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CaseInventory> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            CaseInventory? searchCaseInventory = await context
                .CaseInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            return searchCaseInventory ?? throw new Exception("There is no such case inventory, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<CaseInventory>> GetAll(Guid caseId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            List<CaseInventory> caseInventories = await context
                .CaseInventory
                .Where(x => x.GameCaseId == caseId)
                .ToListAsync();

            return caseInventories;
        }

        public async Task<bool> Create(CaseInventory caseInventory)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameCase? searchGameCase = await context
                .GameCase
                .FirstOrDefaultAsync(x => x.Id == caseInventory.GameCaseId);

            GameItem? searchGameItem = await context
                .GameItem
                .FirstOrDefaultAsync(x => x.Id == caseInventory.GameItemId);

            if (searchGameCase is null) throw new Exception("There is no such case, " +
                "review what data comes from the api");
            if (searchGameItem is null) throw new Exception("There is no such item, " +
                "review what data comes from the api");

            await context.CaseInventory.AddAsync(caseInventory);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Update(CaseInventory caseInventory)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            CaseInventory? searchCaseInventory = await context
                .CaseInventory
                .FirstOrDefaultAsync(x => x.Id == caseInventory.Id);

            if (searchCaseInventory is null) {
                throw new Exception("There is no such case inventory in the database, " +
                    "review what data comes from the api");
            }

            context.Entry(searchCaseInventory).CurrentValues.SetValues(caseInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            CaseInventory? searchCaseInventory = await context
                .CaseInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchCaseInventory is null) {
                throw new Exception("There is no such case inventory in the database, " +
                    "review what data comes from the api");
            }

            context.CaseInventory.Remove(searchCaseInventory);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
