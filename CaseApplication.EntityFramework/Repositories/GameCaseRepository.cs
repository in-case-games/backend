using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class GameCaseRepository : IGameCaseRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public GameCaseRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GameCase> GetCurrentCase(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            GameCase? searchCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            return searchCase ?? throw new Exception("There is no such case, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<GameCase>> GetAllCases()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.GameCase.ToListAsync();
        }

        public async Task<bool> CreateCase(GameCase gameCase)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            await _context.GameCase.AddAsync(gameCase);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCase(GameCase gameCase)
        {
            GameCase searchGameCase = await GetCurrentCase(gameCase.Id);

            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            _context.Entry(searchGameCase).CurrentValues.SetValues(gameCase);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCase(Guid id)
        {
            GameCase searchGameCase = await GetCurrentCase(id);

            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            
            _context.GameCase.Remove(searchGameCase);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
