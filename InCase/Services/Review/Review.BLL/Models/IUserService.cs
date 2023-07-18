using Infrastructure.MassTransit.User;

namespace Review.BLL.Models
{
    public interface IUserService
    {
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
