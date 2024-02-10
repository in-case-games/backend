using Infrastructure.MassTransit.User;

namespace Payment.BLL.Interfaces;
public interface IUserService
{
    public Task CreateAsync(UserTemplate template, CancellationToken cancellation = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}