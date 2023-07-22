using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
        };
    }
}
