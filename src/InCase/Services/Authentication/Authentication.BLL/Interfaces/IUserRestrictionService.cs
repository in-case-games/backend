using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Interfaces;
public interface IUserRestrictionService
{
    public Task<UserRestriction?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    public Task CreateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default);
    public Task UpdateAsync(UserRestrictionTemplate template, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}