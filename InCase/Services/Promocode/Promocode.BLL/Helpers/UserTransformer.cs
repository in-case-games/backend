using Infrastructure.MassTransit.User;
using Promocode.BLL.Models;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User entity) => new()
        {
            Id = entity.Id,
        };

        public static List<UserResponse> ToResponse(this List<User> entities)
        {
            List<UserResponse> response = new();

            foreach (var entity in entities)
                response.Add(entity.ToResponse());

            return response;
        }

        public static User ToEntity(this UserRequest request, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : request.Id,
        };

        public static UserRequest ToRequest(this UserTemplate template, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : template.Id,
        };
    }
}
