using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository : IBaseRepository<UserAdditionalInfo>
    {
        public Task<UserAdditionalInfo?> GetByUserId(Guid userId);
    }
}
