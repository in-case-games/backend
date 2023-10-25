using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace EmailSender.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task<User?> GetAsync(string email);
        public Task CreateAsync(UserTemplate template);
        public Task UpdateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
