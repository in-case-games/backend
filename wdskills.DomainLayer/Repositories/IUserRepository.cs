using wdskills.DomainLayer.Entities;

namespace wdskills.DomainLayer.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> CreateUser(User user);
        public Task<bool> UpdateUser(Guid id, User user);
        public Task<bool> DeleteUser(Guid id);
        public Task<User> GetUser(string email);
        public Task<IEnumerable<User>> GetAllUsers();
    }
}
