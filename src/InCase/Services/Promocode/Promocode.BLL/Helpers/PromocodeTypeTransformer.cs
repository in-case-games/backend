using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers;
public static class PromoCodeTypeTransformer
{
    public static List<PromoCodeTypeResponse> ToResponse(this List<PromoCodeType> types) =>
        types.Select(type => new PromoCodeTypeResponse { Id = type.Id, Name = type.Name, }).ToList();
}