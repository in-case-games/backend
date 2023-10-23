using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<GuestModeResponse> GetGuestModeAsync(Guid userId);
        public Task<BalanceResponse> GetBalanceAsync(Guid userId);
        public Task<BalanceResponse> ChangeBalanceByOwnerAsync(Guid userId, decimal balance);
        public Task<GuestModeResponse> ChangeGuestModeAsync(Guid userId);
    }
}
