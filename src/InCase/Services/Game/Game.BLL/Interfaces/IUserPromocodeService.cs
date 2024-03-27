using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Interfaces;
public interface IUserPromoCodeService
{
    public Task<UserPromoCode?> GetAsync(Guid userId, CancellationToken cancellation = default);
    public Task CreateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default);
    public Task UpdateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default);
}