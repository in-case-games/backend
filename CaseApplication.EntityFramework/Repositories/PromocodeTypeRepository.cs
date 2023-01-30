using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories;

public class PromocodeTypeRepository: IPromocodeTypeRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public PromocodeTypeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task<PromocodeType> Get(Guid id)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        return await context.PromocodeType
            .FirstOrDefaultAsync(x => x.Id == id) ?? 
            throw new Exception("There is no such promocodeType in the database, " +
                               "review what data comes from the api");
    }

    public async Task<bool> Create(PromocodeType entity)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();
        
        entity.Id = Guid.NewGuid();
        
        await context.PromocodeType.AddAsync(entity);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(PromocodeType entity)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        PromocodeType? promocodeType = await context
            .PromocodeType
            .FirstOrDefaultAsync(x => x.Id == entity.Id);
        
        if (promocodeType is null) 
            throw new Exception("PromocodeType, which you search, is not found!");
        
        context.Entry(promocodeType).CurrentValues.SetValues(entity);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        PromocodeType? promocodeType = await context
            .PromocodeType
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (promocodeType is null) 
            throw new Exception("PromocodeType, which you search, is not found!");

        context.PromocodeType.Remove(promocodeType);
        await context.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<PromocodeType>> GetAll()
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        return await context.PromocodeType.ToListAsync();
    }

    public async Task<PromocodeType> GetByName(string name)
    {
        using ApplicationDbContext context = _contextFactory.CreateDbContext();

        return await context.PromocodeType
                   .FirstOrDefaultAsync(x => x.PromocodeTypeName == name) ?? 
               throw new Exception("There is no such promocodeType in the database, " +
                                   "review what data comes from the api");
    }
}