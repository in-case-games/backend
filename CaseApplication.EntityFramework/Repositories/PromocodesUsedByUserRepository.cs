using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.EntityFramework.Repositories;

public class PromocodesUsedByUserRepository: IPromocodeUserByUserRepository
{
    public Task<PromocodesUsedByUser> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(PromocodesUsedByUser entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(PromocodesUsedByUser entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PromocodesUsedByUser>> GetAll(Guid userId)
    {
        throw new NotImplementedException();
    }
}