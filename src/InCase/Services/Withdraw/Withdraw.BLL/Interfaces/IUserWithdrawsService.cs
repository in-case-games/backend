using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces;

public interface IUserWithdrawsService
{
    public Task<UserHistoryWithdrawResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<UserHistoryWithdrawResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default);
    public Task<List<UserHistoryWithdrawResponse>> GetAsync(int count, CancellationToken cancellation = default);
    public Task<UserInventoryResponse> TransferAsync(Guid id, Guid userId, CancellationToken cancellation = default);
}