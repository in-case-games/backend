using Authentication.BLL.Models;
using Authentication.DAL.Entities;

namespace Authentication.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserRequest request, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : request.Id,
            Email = request.Email,
            Login = request.Login
        };

        public static UserResponse ToResponse(this User user, bool IsNewGuid = false) => new() 
        { 
            Id = IsNewGuid ? Guid.NewGuid() : user.Id,
            Email = user.Email,
            Login = user.Login,
        };
    }
}
