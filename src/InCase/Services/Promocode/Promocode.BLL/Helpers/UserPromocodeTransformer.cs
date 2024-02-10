using Infrastructure.MassTransit.User;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers;
public static class UserPromoCodeTransformer
{
    public static UserPromoCodeTemplate ToTemplate(this UserPromoCode entity) => new()
    {
       Id = entity.Id,
       Discount = entity.PromoCode!.Discount,
       Type = entity.PromoCode?.Type is null ? null : new PromoCodeTypeTemplate
       {
           Id = entity.PromoCode.Type.Id,
           Name = entity.PromoCode?.Type.Name,
       },
       UserId = entity.UserId,
    };
}