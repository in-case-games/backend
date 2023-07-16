using Authentication.BLL.Models;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Helpers
{
    public static class UserRestrictionTransformer
    {
        public static UserRestrictionResponse ToResponse(this UserRestriction entity) => new()
        {
            Id = entity.Id,
            ExpirationDate = entity.ExpirationDate,
            UserId = entity.UserId,
        };

        public static UserRestriction ToEntity(
            this UserRestrictionRequest request, 
            bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            ExpirationDate = request.ExpirationDate,
            UserId = request.UserId
        };

        public static UserRestrictionRequest ToRequest(
            this UserRestrictionTemplate template, 
            bool isNewGuid = false) => new()
            {
                Id = isNewGuid ? Guid.NewGuid() : template.Id,
                ExpirationDate = template.ExpirationDate,
                UserId = template.UserId
            };
    }
}
