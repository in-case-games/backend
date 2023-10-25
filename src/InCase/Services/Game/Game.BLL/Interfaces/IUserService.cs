using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
