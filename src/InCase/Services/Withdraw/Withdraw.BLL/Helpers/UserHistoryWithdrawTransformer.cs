using Withdraw.BLL.Models;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Helpers
{
    public static class UserHistoryWithdrawTransformer
    {
        public static UserHistoryWithdrawResponse ToResponse(this UserHistoryWithdraw withdraw) =>
            new()
            {
                Id = withdraw.Id,
                Date = withdraw.Date,
                FixedCost = withdraw.FixedCost,
                InvoiceId = withdraw.InvoiceId,
                ItemId = withdraw.Item?.Id ?? withdraw.ItemId,
                MarketId = withdraw.Market?.Id ?? withdraw.MarketId,
                Status = withdraw?.Status?.Name 
            };

        public static List<UserHistoryWithdrawResponse> ToResponse(
            this List<UserHistoryWithdraw> withdraws)
        {
            List<UserHistoryWithdrawResponse> response = new();

            foreach (var withdraw in withdraws)
                response.Add(withdraw.ToResponse());

            return response;
        }
    }
}
