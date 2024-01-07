using Infrastructure.MassTransit.User;
using Payment.DAL.Entities;

namespace Payment.BLL.Interfaces
{
    public interface IUserPromocodeService
    {
        public Task<UserPromocode?> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default);
        public Task CreateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default);
        public Task UpdateAsync(UserPromocodeTemplate template, CancellationToken cancellation = default);
    }
}
