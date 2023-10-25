using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IWithdrawService
    {
        public Task<UserHistoryWithdrawResponse> WithdrawItemAsync(WithdrawItemRequest request, Guid userId);
        public Task<BalanceMarketResponse> GetMarketBalanceAsync(string marketName);
        public Task WithdrawStatusManagerAsync(int count, CancellationToken cancellationToken);
        public Task<ItemInfoResponse> GetItemInfoAsync(Guid id);
    }
}
