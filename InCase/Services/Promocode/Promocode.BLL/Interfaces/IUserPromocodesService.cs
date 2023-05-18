using Promocode.BLL.Models;

namespace Promocode.BLL.Interfaces
{
    public interface IUserPromocodesService
    {
        public Task<UserHistoryPromocodeResponse> GetAsync(Guid id, Guid userId);
        public Task<List<UserHistoryPromocodeResponse>> GetAsync(Guid userId, int count);
        public Task<List<UserHistoryPromocodeResponse>> GetAsync(int count);
        public Task<UserHistoryPromocodeResponse> ActivateAsync(Guid userId, string name);
        public Task<UserHistoryPromocodeResponse> ExchangeAsync(Guid userId, string name);
    }
}
