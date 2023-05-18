using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class PromocodeTypeTransformer
    {
        public static PromocodeTypeResponse ToResponse(this PromocodeType type) =>
            new()
            {
                Id = type.Id,
                Name = type.Name,
            };

        public static List<PromocodeTypeResponse> ToResponse(this List<PromocodeType> types)
        {
            List<PromocodeTypeResponse> response = new();

            foreach (var type in types)
                response.Add(ToResponse(type));

            return response;
        }
    }
}
