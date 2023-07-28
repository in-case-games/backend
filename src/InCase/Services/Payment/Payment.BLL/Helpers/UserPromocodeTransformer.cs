using Infrastructure.MassTransit.User;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers
{
    public static class UserPromocodeTransformer
    {
        public static UserPromocode ToEntity(this UserPromocodeTemplate template) => new()
        {
            Id = template.Id,
            Discount = template.Discount,
            UserId = template.UserId,
        };

        public static UserPromocodeTemplate ToTemplate(this UserPromocode entity) => new()
        {
            Id = entity.Id,
            Discount = entity.Discount,
            UserId = entity.UserId,
        };
    }
}
