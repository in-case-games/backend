using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository
    {
        public Task<UserAdditionalInfo> GetInfo(Guid userId);
        public Task<bool> CreateInfo(UserAdditionalInfo info);
        public Task<bool> UpdateInfo(UserAdditionalInfo info);
        public Task<bool> DeleteInfo(Guid id);
    }
}
