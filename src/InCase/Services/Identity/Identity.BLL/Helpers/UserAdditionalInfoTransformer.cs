using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Helpers
{
    public static class UserAdditionalInfoTransformer
    {
        public static UserAdditionalInfoResponse ToResponse(this UserAdditionalInfo entity) => new()
        {
            Id = entity.Id,
            CreationDate = entity.CreationDate,
            DeletionDate = entity.DeletionDate,
            UserId = entity.UserId,
            Role = entity.Role?.ToResponse(),
        };

        public static UserAdditionalInfoTemplate ToTemplate(this UserAdditionalInfo entity) => new()
        {
            Id = entity.Id,
            DeletionDate = entity.DeletionDate,
            RoleName = entity.Role?.Name,
            UserId = entity.UserId,
        };
    }
}