using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository
    {
        public Task<bool> CreateInfo(UserAdditionalInfo info);
        public Task<bool> UpdateInfo(UserAdditionalInfo info);
        public Task<bool> DeleteInfo(Guid id);
        public Task<UserAdditionalInfo> GetInfo(Guid id);
        public Task<UserRole> GetRoleToName(string name);
        public Task<UserRole> GetRoleToId(Guid id);
    }
}
