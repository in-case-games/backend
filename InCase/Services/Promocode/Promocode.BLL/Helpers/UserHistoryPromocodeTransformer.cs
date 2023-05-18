using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class UserHistoryPromocodeTransformer
    {
        public static UserHistoryPromocodeResponse ToResponse(this UserHistoryPromocode history) =>
            new() { 
                Id = history.Id,
                Date = history.Date,
                IsActivated = history.IsActivated,
                Discount = TransformDiscount(history),
                Name = history.Promocode?.Name,
                Type = history.Promocode?.Type?.Name
            };

        public static List<UserHistoryPromocodeResponse> ToResponse(
            this List<UserHistoryPromocode> histories)
        {
            List<UserHistoryPromocodeResponse> response = new();

            foreach (var history in histories)
                response.Add(ToResponse(history));

            return response;
        }

        private static int? TransformDiscount(UserHistoryPromocode history) =>
            history.Promocode is null ? null : (int)(history.Promocode.Discount * 100);
    }
}
