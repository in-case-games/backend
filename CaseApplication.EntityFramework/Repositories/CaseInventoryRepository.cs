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
            _context.Set<CaseInventory>().Update(caseInventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
