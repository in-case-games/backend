using Infrastructure.MassTransit.User;
using Support.DAL.Entities;

namespace Support.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
