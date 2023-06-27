using Game.BLL.Models;
using Game.DAL.Entities;

namespace Game.BLL.Helpers
{
    public static class UserAdditionalInfoTransformer
    {
        public static BalanceResponse ToBalanceResponse(this UserAdditionalInfo info) => new()
        {
            Balance = info.Balance,
        };

        public static GuestModeResponse ToGuestModeResponse(this UserAdditionalInfo info) => new()
        {
            IsGuestMode = info.IsGuestMode,
        };
    }
}
