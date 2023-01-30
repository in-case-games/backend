using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class PromocodeRepository: IPromocodeRepository
    {
        private IDbContextFactory<ApplicationDbContext> _contextFactory;

        public PromocodeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Promocode> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
        
            return await context.Promocode.FirstOrDefaultAsync(x => x.Id == id) ?? 
                throw new("There is no such promocode in the database, " +
                    "review what data comes from the api");
        }

        public async Task<Promocode> GetByName(string name)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.Promocode.FirstOrDefaultAsync(x => x.PromocodeName == name) ?? 
                throw new("There is no such promocode in the database, " +
                    "review what data comes from the api");
        }

        public async Task<bool> Create(Promocode promocode)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            promocode.Id = new Guid();

            await context.Promocode.AddAsync(promocode);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(Promocode promocode)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            Promocode? searchPromocode = await context
                .Promocode
                .FirstOrDefaultAsync(x => x.Id == promocode.Id);

            if (searchPromocode is null) throw new("There is no such promocode in the database, " +
                "review what data comes from the api");

            context.Entry(searchPromocode).CurrentValues.SetValues(searchPromocode);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            Promocode? searchPromocode = await context
                .Promocode
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchPromocode is null) throw new("There is no such promocode in the database, " +
                "review what data comes from the api");

            context.Promocode.Remove(searchPromocode);
            await context.SaveChangesAsync();

            return true;
        }
    }
}