using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Repositories
{
    public interface IUserRepository
    {
        public Task<User> Get(string email);
        public Task<User> GetByLogin(string login);
        public Task<User> GetById(Guid id);
        public Task<IEnumerable<User>> GetAll();
        public Task<bool> Create(User user);
        public Task<bool> Update(User user);
        public Task<bool> Delete(Guid id);
    }
}
