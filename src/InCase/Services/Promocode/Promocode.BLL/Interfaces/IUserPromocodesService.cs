using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces
{
    public interface IUserPromocodesService
    {
        public Task<UserPromocodeResponse> GetAsync(Guid id);
        public Task<UserPromocodeResponse> GetAsync(Guid id, Guid userId);
        public Task<List<UserPromocodeResponse>> GetAsync(Guid userId, int count);
        public Task<List<UserPromocodeResponse>> GetAsync(int count);
        public Task<UserPromocodeResponse> ActivateAsync(Guid userId, string name);
        public Task<UserPromocodeResponse> ExchangeAsync(Guid userId, string name);
    }
}
