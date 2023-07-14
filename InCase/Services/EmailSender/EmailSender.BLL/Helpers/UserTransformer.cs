using EmailSender.BLL.MassTransit.Models;
using EmailSender.BLL.Models;
using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User entity) => new()
        {
            Id = entity.Id,
            Email = entity.Email,
        };

        public static User ToEntity(this UserRequest request, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : request.Id,
            Email = request.Email,
        };

        public static UserRequest ToRequest(this UserTemplate template, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : template.Id,
            Email = template.Email,
        };
    }
}
