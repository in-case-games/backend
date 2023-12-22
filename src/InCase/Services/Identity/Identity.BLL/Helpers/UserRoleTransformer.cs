using Identity.BLL.Models;
using Identity.DAL.Entities;

namespace Identity.BLL.Helpers
{
    public static class UserRoleTransformer
    {
        public static UserRoleResponse ToResponse(this UserRole role) => new()
        {
            Id = role.Id,
            Name = role.Name,
        };

        public static List<UserRoleResponse> ToResponse(this List<UserRole> roles) =>
            roles.Select(ToResponse).ToList();
    }
}
