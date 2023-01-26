using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository
    {
        public Task<UserAdditionalInfo> Get(Guid userId);
        public Task<bool> Create(UserAdditionalInfo info);
        public Task<bool> Update(UserAdditionalInfo info);
        public Task<bool> Delete(Guid id);
    }
}
