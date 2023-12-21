using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class PromocodeTransformer
    {
        public static PromocodeResponse ToResponse(this PromocodeEntity promocode) =>
            new()
            {
                Id = promocode.Id,
                NumberActivations = promocode.NumberActivations,
                Discount = promocode.Discount,
                ExpirationDate = promocode.ExpirationDate,
                Name = promocode.Name,
                Type = promocode.Type
            };

        public static List<PromocodeResponse> ToResponse(this List<PromocodeEntity> promocodes) => 
            promocodes.Select(ToResponse).ToList();

        public static PromocodeEntity ToEntity(this PromocodeRequest request, bool IsNewGuid = false) =>
            new()
            {
                Id = IsNewGuid ? Guid.NewGuid() : request.Id,
                Discount = request.Discount,
                ExpirationDate = request.ExpirationDate,
                NumberActivations = request.NumberActivations,
                Name = request.Name,
                TypeId = request.TypeId
            };
    }
}
