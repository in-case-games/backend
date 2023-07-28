using Infrastructure.MassTransit.User;
using Review.DAL.Entities;

namespace Review.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id);
        public Task CreateAsync(UserTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
