using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class CaseInventoryRepository : ICaseInventoryRepository
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        public CaseInventoryRepository(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> CaseInventoryUpdate(CaseInventory caseInventory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            _context.Set<CaseInventory>().Update(caseInventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
