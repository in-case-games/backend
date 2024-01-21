using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace EmailSender.BLL.Interfaces;

public interface IUserService
{
    public Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<User?> GetAsync(string email, CancellationToken cancellationToken = default);
    public Task CreateAsync(UserTemplate template, CancellationToken cancellationToken = default);
    public Task UpdateAsync(UserTemplate template, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}