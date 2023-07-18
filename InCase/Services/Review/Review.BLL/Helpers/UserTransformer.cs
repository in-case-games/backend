using Infrastructure.MassTransit.User;
using Review.DAL.Entities;

namespace Review.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id
        };
    }
}
