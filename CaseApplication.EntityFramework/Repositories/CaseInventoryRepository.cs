using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class CaseInventoryRepository : ICaseInventoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<CaseInventoryDto, CaseInventory>();
        });
        public CaseInventoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CaseInventory?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? caseInventory = await context.CaseInventory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if(caseInventory != null)
            {
                caseInventory.GameItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(
                    x => x.Id == caseInventory.GameItemId);
            }

            return caseInventory;
        }

        public async Task<CaseInventory?> GetById(Guid caseId, Guid itemId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? caseInventory = await context.CaseInventory
                .AsNoTracking().FirstOrDefaultAsync(
                x => x.GameCaseId == caseId && x.GameItemId == itemId);

            if (caseInventory != null)
            {
                caseInventory.GameItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(
                    x => x.Id == caseInventory.GameItemId);
            }

            return caseInventory;
        }

        public async Task<List<CaseInventory>> GetAll(Guid caseId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<CaseInventory> caseInventories = await context.CaseInventory
                .AsNoTracking()
                .Where(x => x.GameCaseId == caseId)
                .ToListAsync();

            foreach(CaseInventory caseInventory in caseInventories)
            {
                caseInventory.GameItem = await context.GameItem
                .AsNoTracking().FirstOrDefaultAsync(
                    x => x.Id == caseInventory.GameItemId);
            }

            return caseInventories;
        }

        public async Task<bool> Create(CaseInventoryDto caseInventoryDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper(); 

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory caseInventory = mapper.Map<CaseInventory>(caseInventoryDto);

            await context.CaseInventory.AddAsync(caseInventory);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Update(CaseInventoryDto caseInventoryDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? oldCaseInventory = await context.CaseInventory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == caseInventoryDto.Id);

            if (oldCaseInventory is null) 
                throw new Exception("There is no such case inventory in the database, " +
                    "review what data comes from the api");

            CaseInventory newCaseInventory = mapper.Map<CaseInventory>(caseInventoryDto);

            context.Entry(oldCaseInventory).CurrentValues.SetValues(newCaseInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            CaseInventory? searchCaseInventory = await context
                .CaseInventory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchCaseInventory is null) 
                throw new Exception("There is no such case inventory in the database, " +
                    "review what data comes from the api");

            context.CaseInventory.Remove(searchCaseInventory);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
