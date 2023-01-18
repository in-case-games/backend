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

        public async Task<bool> CaseInventoryUpdate(CaseInventory caseInventory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            CaseInventory? searchCaseInventory = await _context.CaseInventory.FirstOrDefaultAsync(x => x.Id == caseInventory.Id);

            if (searchCaseInventory is null) throw new Exception("There is no such case inventory, " +
                "review what data comes from the api");

            _context.Entry(searchCaseInventory).CurrentValues.SetValues(caseInventory);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
