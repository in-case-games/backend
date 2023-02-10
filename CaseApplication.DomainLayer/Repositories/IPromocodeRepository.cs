using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IPromocodeRepository
    {
        public Task<Promocode?> Get(Guid id);
        public Task<Promocode?> GetByName(string name);
        public Task<bool> Create(PromocodeDto promocodeDto);
        public Task<bool> Update(PromocodeDto promocodeDto);
        public Task<bool> Delete(Guid id);
    }
}