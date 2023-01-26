using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRoleRepository
    {
        public Task<UserRole> Get(UserRole role);
        public Task<IEnumerable<UserRole>> GetAll();
        public Task<bool> Create(UserRole role);
        public Task<bool> Update(UserRole role);
        public Task<bool> Delete(Guid id);
    }
}
