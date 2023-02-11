using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class PromocodeRepository: IPromocodeRepository
    {
        private IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<PromocodeDto, Promocode>();
        });
        public PromocodeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Promocode?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.Promocode
                .AsNoTracking()
                .Include(x => x.PromocodeType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Promocode?> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.Promocode
                .AsNoTracking()
                .Include(x => x.PromocodeType)
                .FirstOrDefaultAsync(x => x.PromocodeName == name); ;
        }

        public async Task<bool> Create(PromocodeDto promocodeDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            Promocode promocode = mapper.Map<Promocode>(promocodeDto);

            promocode.Id = new Guid();

            await context.Promocode.AddAsync(promocode);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(PromocodeDto promocodeDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? oldPromocode = await context.Promocode
                .FirstOrDefaultAsync(x => x.Id == promocodeDto.Id);

            if (oldPromocode is null) throw new("There is no such promocode in the database, " +
                "review what data comes from the api");

            IMapper? mapper = _mapperConfiguration.CreateMapper();
            Promocode newPromocode = mapper.Map<Promocode>(promocodeDto);

            context.Entry(oldPromocode).CurrentValues.SetValues(newPromocode);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            Promocode? searchPromocode = await context
                .Promocode
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchPromocode is null) throw new("There is no such promocode in the database, " +
                "review what data comes from the api");

            context.Promocode.Remove(searchPromocode);
            await context.SaveChangesAsync();

            return true;
        }
    }
}