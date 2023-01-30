using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories;

public interface IPromocodeTypeRepository: IBaseRepository<PromocodeType>
{
    public Task<IEnumerable<PromocodeType>> GetAll();
    public Task<PromocodeType> GetByName(string name);
}