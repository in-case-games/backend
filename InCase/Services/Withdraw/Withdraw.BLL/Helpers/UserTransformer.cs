using Infrastructure.MassTransit.User;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Helpers
{
    public static class UserTransformer
    {
        public static User ToEntity(this UserTemplate template) => new()
        {
            Id = template.Id,
        };
    }
}
