using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class UserHistoryPromocodeTransformer
    {
        public static UserPromocodeResponse ToResponse(this UserPromocode history) =>
            new() { 
                Id = history.Id,
                Date = history.Date,
                IsActivated = history.IsActivated,
                Discount = TransformDiscount(history),
                Name = history.Promocode?.Name,
                Type = history.Promocode?.Type?.Name
            };

        public static List<UserPromocodeResponse> ToResponse(
            this List<UserPromocode> histories)
        {
            List<UserPromocodeResponse> response = new();

            foreach (var history in histories)
                response.Add(ToResponse(history));

            return response;
        }

        private static int? TransformDiscount(UserPromocode history) =>
            history.Promocode is null ? null : (int)(history.Promocode.Discount * 100);
    }
}
