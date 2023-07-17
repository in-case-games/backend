using Infrastructure.MassTransit.User;
using Payment.BLL.Models;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers
{
    public static class UserPromocodeTransformer
    {
        public static UserPromocodeResponse ToResponse(this UserPromocode entity) => new()
        {
            Id = entity.Id,
            Discount = entity.Discount,
            UserId = entity.UserId,
        };

        public static UserPromocode ToEntity(this UserPromocodeRequest request, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Discount = request.Discount,
            UserId = request.UserId,
        };

        public static UserPromocodeRequest ToRequest(this UserPromocodeTemplate template, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : template.Id,
            Discount = template.Discount,
            UserId = template.UserId,
        };

        public static UserPromocodeTemplate ToTemplate(this UserPromocode entity, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : entity.Id,
            Discount = entity.Discount,
            UserId = entity.UserId
        };
    }
}
