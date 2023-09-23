using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<GuestModeResponse> GetGuestModeAsync(Guid userId);
        public Task<BalanceResponse> GetBalanceAsync(Guid userId);
        public Task<BalanceResponse> TopUpBalanceByOwnerAsync(Guid userId, decimal amount);
        public Task<GuestModeResponse> ChangeGuestModeAsync(Guid userId);
    }
}
