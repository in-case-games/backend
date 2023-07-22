using Infrastructure.MassTransit.User;
using Payment.DAL.Entities;

namespace Payment.BLL.Interfaces
{
    public interface IUserPromocodeService
    {
        public Task<UserPromocode?> GetAsync(Guid id, Guid userId);
        public Task CreateAsync(UserPromocodeTemplate template);
        public Task UpdateAsync(UserPromocodeTemplate template);
    }
}
