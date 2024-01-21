using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Interfaces;

public interface IUserPromocodeService
{
    public Task<UserPromocode?> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task CreateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default);
    public Task UpdateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default);
}