using Infrastructure.MassTransit.User;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
        };
    }
}
