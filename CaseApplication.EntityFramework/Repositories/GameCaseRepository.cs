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

        public async Task<GameCase?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameCase.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<GameCase?> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameCase.FirstOrDefaultAsync(x => x.GameCaseName == name);
        }

        public async Task<List<GameCase>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameCase.ToListAsync();
        }

        public async Task<List<GameCase>> GetAllByGroupName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.GameCase
                .Where(x => x.GroupCasesName == name)
                .ToListAsync();
        }

        public async Task<bool> Create(GameCase gameCase)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            await context.GameCase.AddAsync(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(GameCase gameCase)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? searchCase = await context.GameCase.FirstOrDefaultAsync(x => x.Id == gameCase.Id);

            if (searchCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.Entry(searchCase).CurrentValues.SetValues(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? searchCase = await context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            if (searchCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.GameCase.Remove(searchCase);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
