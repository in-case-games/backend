using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRoleRepository
    {
        public Task<UserRole> GetRole(UserRole role);
        public Task<IEnumerable<UserRole>> GetAllRoles();
        public Task<bool> CreateRole(UserRole role);
        public Task<bool> UpdateRole(UserRole role);
        public Task<bool> DeleteRole(Guid id);
    }
}
