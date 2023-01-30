using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;

namespace CaseApplication.EntityFramework.Repositories;

public class PromocodeRepository: IPromocodeRepository
{
    public Task<Promocode> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(Promocode entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Promocode entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Promocode> GetByName(string name)
    {
        throw new NotImplementedException();
    }
}