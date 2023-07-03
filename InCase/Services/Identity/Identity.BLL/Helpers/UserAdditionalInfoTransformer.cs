
using Identity.BLL.Models;
using Identity.DAL.Entities;

namespace Identity.BLL.Helpers
{
    public static class UserAdditionalInfoTransformer
    {
        public static UserAdditionalInfo ToEntity(this UserAdditionalInfoRequest request) => new()
        {
            Id = request.Id,
            CreationDate = request.CreationDate,
            DeletionDate = request.DeletionDate,
            RoleId = request.RoleId,
            UserId = request.UserId,
        };

        public static UserAdditionalInfoResponse ToResponse(this UserAdditionalInfo entity) => new()
        {
            Id = entity.Id,
            CreationDate = entity.CreationDate,
            DeletionDate = entity.DeletionDate,
            UserId = entity.UserId,
            Role = entity.Role?.ToResponse(),
        };
    }
}