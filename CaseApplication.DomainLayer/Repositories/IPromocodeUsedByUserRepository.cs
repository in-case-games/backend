using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IPromocodeUsedByUserRepository
    {
        public Task<PromocodesUsedByUser?> Get(Guid id);
        public Task<List<PromocodesUsedByUser>> GetAll(Guid userId);
        public Task<bool> Create(PromocodesUsedByUserDto promocodesUsedDto);
        public Task<bool> Update(PromocodesUsedByUserDto promocodesUsedDto);
        public Task<bool> Delete(Guid id);
    }
}