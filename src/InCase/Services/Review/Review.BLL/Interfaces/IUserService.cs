using Infrastructure.MassTransit.User;
using Review.DAL.Entities;

namespace Review.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetAsync(Guid id, CancellationToken cancellation = default);
        public Task CreateAsync(UserTemplate templat, CancellationToken cancellation = default);
        public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
