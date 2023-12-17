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

        public static List<UserRoleResponse> ToResponse(this List<UserRole> roles)
        {
            var response = new List<UserRoleResponse>();

            foreach(UserRole role in roles) response.Add(ToResponse(role));

            return response;
        }
    }
}
