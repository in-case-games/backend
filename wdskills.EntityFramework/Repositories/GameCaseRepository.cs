using Microsoft.EntityFrameworkCore;
using wdskills.DomainLayer.Entities;
using wdskills.DomainLayer.Repositories;
using wdskills.EntityFramework.Data;

namespace wdskills.EntityFramework.Repositories
{
    public class GameCaseRepository : IGameCaseRepository
    {
        private readonly ApplicationDbContextFactory _contextFactory;

        public GameCaseRepository(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> AddItem(Guid id, GameItem item)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            GameCase? gameCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id);
            if(gameCase != null)
            {
                CaseInventory caseInventory = new()
                {
                    GameCase = gameCase,
                    CaseItem = item
                };
                await _context.CaseInventory.AddAsync(caseInventory);
                await _context.SaveChangesAsync();
            }
            return (gameCase != null);
        }

        public async Task<bool> CreateCase(GameCase gameCase)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            await _context.GameCase.AddAsync(gameCase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCase(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            GameCase? gameCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id);
            if(gameCase != null)
            {
                _context.GameCase.Remove(gameCase);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<GameCase>> GetAllCases()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.GameCase.ToListAsync();
        }

        public async Task<GameCase> GetCurrentCase(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id) ?? new();
        }

        public async Task<bool> RemoveItem(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            CaseInventory? caseInventory = await _context.CaseInventory.FirstOrDefaultAsync(x => x.Id == id);
            if(caseInventory != null)
            {
                _context.CaseInventory.Remove(caseInventory);
                await _context.SaveChangesAsync();
            }
            return (caseInventory != null);
        }

        public async Task<bool> UpdateCase(GameCase gameCase)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            _context.Set<GameCase>().Update(gameCase);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
