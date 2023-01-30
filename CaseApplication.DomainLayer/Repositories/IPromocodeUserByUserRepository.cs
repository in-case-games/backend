using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IPromocodeUserByUserRepository: IBaseRepository<PromocodesUsedByUser>
    {
        public Task<IEnumerable<PromocodesUsedByUser>> GetAll(Guid userId);
    }
}