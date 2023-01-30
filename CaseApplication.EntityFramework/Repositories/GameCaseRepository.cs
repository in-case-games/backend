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

        public async Task<GameCase> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameCase? searchCase = await context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            return searchCase ?? throw new Exception("There is no such case, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<GameCase>> GetAll()
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.GameCase.ToListAsync();
        }

        public async Task<IEnumerable<GameCase>> GetAllByGroupName(string name)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<GameCase> gameCases = await context
                .GameCase
                .Where(x => x.GroupCasesName == name)
                .ToListAsync();

            return gameCases;
        }

        public async Task<bool> Create(GameCase gameCase)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            await context.GameCase.AddAsync(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(GameCase gameCase)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameCase? searchCase = await context.GameCase.FirstOrDefaultAsync(x => x.Id == gameCase.Id);

            if (searchCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.Entry(searchCase).CurrentValues.SetValues(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameCase? searchCase = await context.GameCase.FirstOrDefaultAsync(x => x.Id == id);

            if (searchCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.GameCase.Remove(searchCase);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
