using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserWithdrawsService
    {
        public Task<UserHistoryWithdrawResponse> GetAsync(Guid id);
        public Task<List<UserHistoryWithdrawResponse>> GetAsync(Guid userId, int count);
        public Task<List<UserHistoryWithdrawResponse>> GetAsync(int count);
        public Task<UserInventoryResponse> TransferAsync(Guid id, Guid userId);
    }
}
