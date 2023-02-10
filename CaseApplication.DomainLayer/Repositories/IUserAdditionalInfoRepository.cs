using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserAdditionalInfoRepository
    {
        public Task<UserAdditionalInfo?> Get(Guid id);
        public Task<UserAdditionalInfo?> GetByUserId(Guid userId);
        public Task<bool> Create(UserAdditionalInfoDto infoDto);
        public Task<bool> Update(UserAdditionalInfo info);
        public Task<bool> Delete(Guid id);
    }
}
