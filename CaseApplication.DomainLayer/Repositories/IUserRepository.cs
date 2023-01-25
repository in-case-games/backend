using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string email);
        public Task<User> GetUserByLogin(string login);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<bool> CreateUser(User user);
        public Task<bool> UpdateUser(User user);
        public Task<bool> DeleteUser(Guid id);
    }
}
