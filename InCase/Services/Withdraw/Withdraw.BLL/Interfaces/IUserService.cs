using Infrastructure.MassTransit.User;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserService
    {
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
