using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using AutoMapper;
using CaseApplication.DomainLayer.Dtos;

namespace CaseApplication.EntityFramework.Repositories
{
    public class GameCaseRepository : IGameCaseRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<GameCaseDto, GameCase>();
        });
        public GameCaseRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GameCase?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if(gameCase != null)
            {
                gameCase.СaseInventories = await context.CaseInventory
                .AsNoTracking()
                    .Where(x => x.GameCaseId == gameCase.Id)
                    .ToListAsync();

                //TODO Check efficiency
                foreach(CaseInventory caseInventory in gameCase.СaseInventories)
                {
                    caseInventory.GameItem = await context.GameItem
                        .AsNoTracking().FirstOrDefaultAsync(
                        x => x.Id == caseInventory.GameItemId);
                }
            }

            return gameCase;
        }
        public async Task<GameCase?> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? gameCase = await context.GameCase
                .AsNoTracking().FirstOrDefaultAsync(x => x.GameCaseName == name);

            if (gameCase != null)
            {
                gameCase.СaseInventories = await context.CaseInventory
                    .AsNoTracking()
                    .Where(x => x.GameCaseId == gameCase.Id)
                    .ToListAsync();
            }

            return gameCase;
        }

        public async Task<List<GameCase>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking().ToListAsync();

            //TODO revise the method may not need to get
/*            foreach (GameCase gameCase in gameCases)
            {
                gameCase.СaseInventories = await context.CaseInventory
                    .Where(x => x.GameCaseId == gameCase.Id)
                    .ToListAsync();
            }*/

            return gameCases;
        }

        public async Task<List<GameCase>> GetAllByGroupName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<GameCase> gameCases = await context.GameCase
                .AsNoTracking()
                .Where(x => x.GroupCasesName == name)
                .ToListAsync();

            //TODO revise the method may not need to get
/*            foreach (GameCase gameCase in gameCases)
            {
                gameCase.СaseInventories = await context.CaseInventory
                    .Where(x => x.GameCaseId == gameCase.Id)
                    .ToListAsync();
            }*/

            return gameCases;
        }

        public async Task<bool> Create(GameCaseDto gameCaseDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase gameCase = mapper.Map<GameCase>(gameCaseDto);

            await context.GameCase.AddAsync(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(GameCase gameCase)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? oldCase = await context.GameCase
                .FirstOrDefaultAsync(x => x.Id == gameCase.Id);

            if (oldCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.Entry(oldCase).CurrentValues.SetValues(gameCase);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            GameCase? searchCase = await context.GameCase
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchCase is null) throw new Exception("There is no such case in the database, " +
                "review what data comes from the api");

            context.GameCase.Remove(searchCase);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
