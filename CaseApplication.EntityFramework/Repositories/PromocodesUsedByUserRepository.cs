using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories;

public class PromocodesUsedByUserRepository: IPromocodeUserByUserRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public PromocodesUsedByUserRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<PromocodesUsedByUser> Get(Guid id)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();
        
        return await context
            .PromocodeUsedByUsers.FirstOrDefaultAsync(x => x.Id == id) ??
               throw new Exception("There is no such PromocodesUsedByUser in the database, " +
                         "review what data comes from the api");
    }

    public async Task<bool> Create(PromocodesUsedByUser promocodesUsedByUser)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        promocodesUsedByUser.Id = new Guid();
        
        await context.PromocodeUsedByUsers.AddAsync(promocodesUsedByUser);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(PromocodesUsedByUser promocodesUsedByUser)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        PromocodesUsedByUser? searchPromocode = await context
            .PromocodeUsedByUsers
            .FirstOrDefaultAsync(x => x.Id == promocodesUsedByUser.Id);

        if (searchPromocode is null) 
            throw new("There is no such PromocodesUsedByUser in the database, " +
                                               "review what data comes from the api");

        context.Entry(searchPromocode).CurrentValues.SetValues(promocodesUsedByUser);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        PromocodesUsedByUser? promocodeType = await context
            .PromocodeUsedByUsers
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (promocodeType is null) 
            throw new Exception("PromocodesUsedByUser, which you search, is not found!");

        context.PromocodeUsedByUsers.Remove(promocodeType);
        await context.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<PromocodesUsedByUser>> GetAll(Guid userId)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();
        
        return await context.PromocodeUsedByUsers
                   .Where(x => x.UserId == userId).ToListAsync() ?? 
               throw new Exception("There is no such PromocodeUsedByUsers in the database, " +
                                   "review what data comes from the api");
    }
}