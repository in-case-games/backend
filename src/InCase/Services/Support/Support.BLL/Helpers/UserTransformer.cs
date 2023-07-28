using Infrastructure.MassTransit.User;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
        };
    }
}
