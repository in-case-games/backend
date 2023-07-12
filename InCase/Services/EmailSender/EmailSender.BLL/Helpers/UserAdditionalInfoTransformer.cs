using EmailSender.BLL.Models;
using EmailSender.DAL.Entities;

namespace EmailSender.BLL.Helpers
{
    public static class UserAdditionalInfoTransformer
    {
        public static UserAdditionalInfoResponse ToResponse(this UserAdditionalInfo entity) => new()
        {
            Id = entity.Id,
            IsNotifyEmail = entity.IsNotifyEmail,
            UserId = entity.UserId,
        };
    }
}
