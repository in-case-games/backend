using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IPromocodeUsedByUserRepository: IBaseRepository<PromocodesUsedByUser>
    {
        public Task<List<PromocodesUsedByUser>> GetAll(Guid userId);
    }
}