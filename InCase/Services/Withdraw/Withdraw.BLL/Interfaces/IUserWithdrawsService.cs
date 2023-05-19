using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserWithdrawsService
    {
        public Task<UserHistoryWithdrawResponse> Get(Guid id);
        public Task<List<UserHistoryWithdrawResponse>> Get(Guid userId, int count);
        public Task<List<UserHistoryWithdrawResponse>> Get(int count);
        public Task<UserInventoryResponse> Transfer(Guid id, Guid userId);
    }
}
