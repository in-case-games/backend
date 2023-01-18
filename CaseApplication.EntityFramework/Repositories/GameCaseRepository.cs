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

        public async Task<bool> AddItem(Guid id, GameItem item)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            GameCase? searchGameCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            if(searchGameCase is null) throw new Exception("There is no such case, " +
                "review what data comes from the api");

            CaseInventory caseInventory = new()
            {
                Id = new Guid(),
                GameCaseId = searchGameCase.Id,
                GameItemId = item.Id,
            };

            await _context.CaseInventory.AddAsync(caseInventory);
            await _context.SaveChangesAsync();

            return true;
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

            GameCase? searchGameCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            if(searchGameCase is null) throw new Exception("There is no such case, " +
                "review what data comes from the api");
            
            _context.GameCase.Remove(searchGameCase);
            await _context.SaveChangesAsync();

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

            CaseInventory? searchCaseInventory = await _context.CaseInventory.FirstOrDefaultAsync(x => x.Id == id);

            if (searchCaseInventory is null) throw new Exception("There is no such case inventory, " +
                "review what data comes from the api");
            
            _context.CaseInventory.Remove(searchCaseInventory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCase(GameCase gameCase)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            GameCase? searchGameCase = await _context.GameCase.FirstOrDefaultAsync(x => x.Id == gameCase.Id);

            if (searchGameCase is null) throw new Exception("There is no such case, " +
                "review what data comes from the api");

            _context.Entry(searchGameCase).CurrentValues.SetValues(gameCase);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
