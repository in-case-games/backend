using Infrastructure.MassTransit.User;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class UserPromocodeTransformer
    {
        public static UserPromocodeTemplate ToTemplate(this UserPromocode entity) => new()
        {
           Id = entity.Id,
           Discount = entity.Promocode!.Discount,
           Type = entity.Promocode?.Type?.ToTemplate(),
           UserId = entity.UserId,
        };
    }
}
