using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IPromocodeTypeRepository: IBaseRepository<PromocodeType>
    {
        public Task<List<PromocodeType>> GetAll();
        public Task<PromocodeType?> GetByName(string name);
    }
}