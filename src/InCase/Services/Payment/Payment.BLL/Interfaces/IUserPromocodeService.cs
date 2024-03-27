using Infrastructure.MassTransit.User;
using Payment.DAL.Entities;

namespace Payment.BLL.Interfaces;
public interface IUserPromoCodeService
{
    public Task<UserPromoCode?> GetAsync(Guid userId, CancellationToken cancellation = default);
    public Task CreateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default);
    public Task UpdateAsync(UserPromoCodeTemplate template, CancellationToken cancellation = default);
}