using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class PromocodeTypeRepository: IPromocodeTypeRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public PromocodeTypeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<PromocodeType?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.PromocodeType
                    .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PromocodeType?> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.PromocodeType
                    .FirstOrDefaultAsync(x => x.PromocodeTypeName == name);
        }

        public async Task<List<PromocodeType>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.PromocodeType.ToListAsync();
        }

        public async Task<bool> Create(PromocodeType promocodeType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
        
            promocodeType.Id = Guid.NewGuid();
        
            await context.PromocodeType.AddAsync(promocodeType);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(PromocodeType promocodeType)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? promoType = await context
                .PromocodeType
                .FirstOrDefaultAsync(x => x.Id == promocodeType.Id);
        
            if (promoType is null) 
                throw new Exception("PromocodeType, which you search, is not found!");
        
            context.Entry(promoType).CurrentValues.SetValues(promocodeType);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodeType? promocodeType = await context
                .PromocodeType
                .FirstOrDefaultAsync(x => x.Id == id);
        
            if (promocodeType is null) 
                throw new Exception("PromocodeType, which you search, is not found!");

            context.PromocodeType.Remove(promocodeType);
            await context.SaveChangesAsync();
        
            return true;
        }
    }
}