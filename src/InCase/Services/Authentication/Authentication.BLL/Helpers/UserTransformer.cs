using Authentication.BLL.Models;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User entity, bool isNewGuid = false) => new() 
        { 
            Id = isNewGuid ? Guid.NewGuid() : entity.Id,
            Email = entity.Email,
            Login = entity.Login,
        };

        public static UserTemplate ToTemplate(this User entity, bool isDeleted) => new()
        {
            Id = entity.Id,
            Email = entity.Email,
            Login = entity.Login,
            IsDeleted = isDeleted
        };
    }
}
