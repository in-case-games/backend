using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories;

public interface IPromocodeRepository: IBaseRepository<Promocode>
{
    public Task<Promocode> GetByName(string name);
}