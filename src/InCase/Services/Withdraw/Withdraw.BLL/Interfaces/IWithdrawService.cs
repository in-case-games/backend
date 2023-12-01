using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IWithdrawService
    {
        public Task<UserHistoryWithdrawResponse> WithdrawItemAsync(WithdrawItemRequest request, Guid userId, CancellationToken cancellation = default);
        public Task<BalanceMarketResponse> GetMarketBalanceAsync(string marketName, CancellationToken cancellation = default);
        public Task WithdrawStatusManagerAsync(int count, CancellationToken cancellation = default);
        public Task<ItemInfoResponse> GetItemInfoAsync(Guid id, CancellationToken cancellation = default);
    }
}
