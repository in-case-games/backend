using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserInventoryService
    {
        public Task<UserInventoryResponse> GetById(Guid id);
        public Task<List<UserInventoryResponse>> Get(Guid userId);
        public Task<List<UserInventoryResponse>> Get(Guid userId, int count);
        public Task<decimal> Sell(Guid id, Guid userId);
        public Task<decimal> SellLast(Guid itemId, Guid userId);
        public Task<UserInventoryResponse> Exchange(Guid id, Guid itemId, Guid userId);
    }
}
