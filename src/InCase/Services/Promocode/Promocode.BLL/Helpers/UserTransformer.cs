using Infrastructure.MassTransit.User;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
        };
    }
}
