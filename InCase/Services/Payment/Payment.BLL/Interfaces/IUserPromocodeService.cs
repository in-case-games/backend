using Infrastructure.MassTransit.User;

namespace Payment.BLL.Interfaces
{
    public interface IUserPromocodeService
    {
        public Task CreateAsync(UserPromocodeTemplate template);
        public Task UpdateAsync(UserPromocodeTemplate template);
    }
}
