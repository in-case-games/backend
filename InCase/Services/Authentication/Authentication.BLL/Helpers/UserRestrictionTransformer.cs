using Authentication.BLL.Models;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Helpers
{
    public static class UserRestrictionTransformer
    {
        public static UserRestriction ToEntity(this UserRestrictionTemplate template) => new()
        {
            Id = template.Id,
            ExpirationDate = template.ExpirationDate,
            UserId = template.UserId
        };
    }
}
