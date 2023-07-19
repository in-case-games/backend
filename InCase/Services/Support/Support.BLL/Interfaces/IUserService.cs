using Infrastructure.MassTransit.User;

namespace Support.BLL.Interfaces
{
    public interface IUserService
    {
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
