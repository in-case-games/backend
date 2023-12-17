using Infrastructure.MassTransit.User;
using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class PromocodeTypeTransformer
    {
        public static List<PromocodeTypeResponse> ToResponse(this List<PromocodeType> types)
        {
            List<PromocodeTypeResponse> response = new();

            foreach (var type in types)
            {
                response.Add(new PromocodeTypeResponse
                {
                    Id = type.Id,
                    Name = type.Name,
                });
            }

            return response;
        }
    }
}
