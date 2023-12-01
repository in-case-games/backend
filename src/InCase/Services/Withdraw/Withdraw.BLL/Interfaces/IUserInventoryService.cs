using Infrastructure.MassTransit.User;
using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserInventoryService
    {
        public Task<UserInventoryResponse> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        public Task<UserInventory?> GetByConsumerAsync(Guid id, CancellationToken cancellation = default);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId, CancellationToken cancellation = default);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default);
        public Task<UserInventoryResponse> CreateAsync(UserInventoryTemplate template, CancellationToken cancellation = default);
        public Task<UserInventoryResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
        public Task<SellItemResponse> SellAsync(Guid id, Guid userId, CancellationToken cancellation = default);
        public Task<SellItemResponse> SellLastAsync(Guid itemId, Guid userId, CancellationToken cancellation = default);
        public Task<List<UserInventoryResponse>> ExchangeAsync(ExchangeItemRequest request, Guid userId, CancellationToken cancellation = default);
    }
}
