using Game.BLL.Models;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Helpers
{
    public static class UserPromocodeTransformer
    {
        public static UserPromocode ToEntity(this UserPromocodeTemplate template) => new()
        {
            Id = template.Id,
            Discount = template.Discount,
            UserId = template.UserId,
        };

        public static UserPromocodeBackTemplate ToTemplate(this UserPromocode entity, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : entity.Id,
        };
    }
}
