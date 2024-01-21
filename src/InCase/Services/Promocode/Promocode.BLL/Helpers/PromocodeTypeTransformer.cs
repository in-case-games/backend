using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers;

public static class PromocodeTypeTransformer
{
    public static List<PromocodeTypeResponse> ToResponse(this List<PromocodeType> types) =>
        types.Select(type => new PromocodeTypeResponse { Id = type.Id, Name = type.Name, }).ToList();
}