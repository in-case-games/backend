using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.EntityFramework.Repositories;

public class PromocodeTypeRepository: IPromocodeTypeRepository
{
    public Task<PromocodeType> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(PromocodeType entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(PromocodeType entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PromocodeType>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<PromocodeType> GetByName(string name)
    {
        throw new NotImplementedException();
    }
}