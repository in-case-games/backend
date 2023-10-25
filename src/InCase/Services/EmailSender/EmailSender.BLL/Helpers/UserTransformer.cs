using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace EmailSender.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
            Email = template.Email,
        };
    }
}
