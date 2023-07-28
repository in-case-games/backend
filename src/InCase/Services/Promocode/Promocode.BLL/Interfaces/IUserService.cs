using Infrastructure.MassTransit.User;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
