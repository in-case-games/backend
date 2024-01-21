using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Interfaces;

public interface IUserService
{
    public Task<UserResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<UserResponse> GetAsync(string login, CancellationToken cancellation = default);
    public Task<User?> GetByConsumerAsync(Guid id, CancellationToken cancellation = default);
    public Task CreateAsync(UserTemplate template, CancellationToken cancellation = default);
    public Task UpdateLoginAsync(UserTemplate template, CancellationToken cancellation = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}