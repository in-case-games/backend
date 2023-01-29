using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        public Task<UserRole> GetByRole(UserRole role);
        public Task<IEnumerable<UserRole>> GetAll();
    }
}
