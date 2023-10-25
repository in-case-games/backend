using Infrastructure.MassTransit.User;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserInventoryService
    {
        public Task<UserInventoryResponse> GetByIdAsync(Guid id);
        public Task<UserInventory?> GetByConsumerAsync(Guid id);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId, int count);
        public Task<UserInventoryResponse> CreateAsync(UserInventoryTemplate template);
        public Task<UserInventoryResponse> DeleteAsync(Guid id);
        public Task<SellItemResponse> SellAsync(Guid id, Guid userId);
        public Task<SellItemResponse> SellLastAsync(Guid itemId, Guid userId);
        public Task<List<UserInventoryResponse>> ExchangeAsync(ExchangeItemRequest request, Guid userId);
    }
}
