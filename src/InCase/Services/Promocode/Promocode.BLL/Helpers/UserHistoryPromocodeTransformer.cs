using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers;
public static class UserHistoryPromoCodeTransformer
{
    public static UserPromoCodeResponse ToResponse(this UserPromoCode history) =>
        new() { 
            Id = history.Id,
            Date = history.Date,
            IsActivated = history.IsActivated,
            Discount = history.PromoCode is null ? null : (int)(history.PromoCode.Discount * 100),
            Name = history.PromoCode?.Name,
            Type = history.PromoCode?.Type?.Name
        };

    public static List<UserPromoCodeResponse> ToResponse(this List<UserPromoCode> histories) => 
        histories.Select(ToResponse).ToList();
}