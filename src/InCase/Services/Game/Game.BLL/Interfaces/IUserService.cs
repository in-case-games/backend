using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Interfaces;
public interface IUserService
{
    public Task<User?> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task CreateAsync(UserTemplate template, CancellationToken cancellation = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}