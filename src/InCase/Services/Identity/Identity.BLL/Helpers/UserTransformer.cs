using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User entity) => new()
        {
            Id = entity.Id,
            AdditionalInfo = entity.AdditionalInfo?.ToResponse(),
            Login = entity.Login,
            Restrictions = entity.Restrictions?.ToResponse(),
            OwnerRestrictions = entity.OwnerRestrictions?.ToResponse(),
        };

        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
            Login = template.Login
        };

        public static UserRequest ToRequest(this UserTemplate template) => new()
        {
            Id = template.Id,
            Login = template.Login
        };
    }
}
