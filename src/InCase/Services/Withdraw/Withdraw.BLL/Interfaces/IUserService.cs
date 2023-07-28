using Infrastructure.MassTransit.User;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
