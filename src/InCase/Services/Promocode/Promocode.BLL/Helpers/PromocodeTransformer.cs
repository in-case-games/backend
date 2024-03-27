using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers;
public static class PromoCodeTransformer
{
    public static PromoCodeResponse ToResponse(this PromoCode promoCode) =>
        new()
        {
            Id = promoCode.Id,
            NumberActivations = promoCode.NumberActivations,
            Discount = promoCode.Discount,
            ExpirationDate = promoCode.ExpirationDate,
            Name = promoCode.Name,
            Type = promoCode.Type
        };

    public static List<PromoCodeResponse> ToResponse(this List<PromoCode> promoCodes) => 
        promoCodes.Select(ToResponse).ToList();

    public static PromoCode ToEntity(this PromoCodeRequest request, bool isNewGuid = false) =>
        new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Discount = request.Discount,
            ExpirationDate = request.ExpirationDate,
            NumberActivations = request.NumberActivations,
            Name = request.Name,
            TypeId = request.TypeId
        };
}