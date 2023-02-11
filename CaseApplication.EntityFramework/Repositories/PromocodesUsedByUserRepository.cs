using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class PromocodesUsedByUserRepository: IPromocodeUsedByUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<PromocodesUsedByUserDto, PromocodesUsedByUserDto>();
        });

        public PromocodesUsedByUserRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PromocodesUsedByUser?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodesUsedByUser? promocodeUsed = await context.PromocodeUsedByUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promocodeUsed != null)
            {
                promocodeUsed.Promocode = await context.Promocode
                .AsNoTracking().FirstOrDefaultAsync
                    (x => x.Id == promocodeUsed.PromocodeId);
            }
            
            return promocodeUsed;
        }

        public async Task<List<PromocodesUsedByUser>> GetAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<PromocodesUsedByUser> promocodesUseds = await context.PromocodeUsedByUsers
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

            foreach(PromocodesUsedByUser promocodeUsed in promocodesUseds)
            {
                promocodeUsed.Promocode = await context.Promocode.FirstOrDefaultAsync
                    (x => x.Id == promocodeUsed.PromocodeId);
            }

            return promocodesUseds;
        }

        public async Task<bool> Create(PromocodesUsedByUserDto promocodesUsedDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodesUsedByUser promocodesUsedByUser = mapper
                .Map<PromocodesUsedByUser>(promocodesUsedDto);

            promocodesUsedByUser.Id = new Guid();
        
            await context.PromocodeUsedByUsers.AddAsync(promocodesUsedByUser);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(PromocodesUsedByUserDto promocodesUsedDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodesUsedByUser? oldPromocodeUsed = await context.PromocodeUsedByUsers
                .FirstOrDefaultAsync(x => x.Id == promocodesUsedDto.Id);

            if (oldPromocodeUsed is null) 
                throw new("There is no such PromocodesUsedByUser in the database, " +
                    "review what data comes from the api");

            PromocodesUsedByUser newPromocodesUsed = mapper
                .Map<PromocodesUsedByUser>(promocodesUsedDto);

            context.Entry(oldPromocodeUsed).CurrentValues.SetValues(newPromocodesUsed);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            PromocodesUsedByUser? promocodeType = await context
                .PromocodeUsedByUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        
            if (promocodeType is null) 
                throw new Exception("PromocodesUsedByUser, which you search, is not found!");

            context.PromocodeUsedByUsers.Remove(promocodeType);
            await context.SaveChangesAsync();
        
            return true;
        }
    }
}