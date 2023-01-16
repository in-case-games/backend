using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository
    {
        public Task<bool> CreateInfo(UserAdditionalInfo info);
        public Task<bool> UpdateInfo(Guid id, UserAdditionalInfo info);
        public Task<bool> DeleteInfo(Guid id);
        public Task<UserAdditionalInfo> GetInfo(Guid id);
    }
}
