using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<GuestModeResponse> GetGuestModeAsync(Guid userId, CancellationToken cancellation = default);
        public Task<BalanceResponse> GetBalanceAsync(Guid userId, CancellationToken cancellation = default);
        public Task<BalanceResponse> ChangeBalanceByOwnerAsync(Guid userId, decimal balance, CancellationToken cancellation = default);
        public Task<GuestModeResponse> ChangeGuestModeAsync(Guid userId, CancellationToken cancellation = default);
    }
}
