using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        public Task<UserRole?> GetByName(string name);
        public Task<List<UserRole>> GetAll();
    }
}
