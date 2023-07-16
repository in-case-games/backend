using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Helpers
{
    public static class UserRestrictionTransformer
    {
        public static RestrictionTypeResponse ToResponse(this RestrictionType entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        public static List<RestrictionTypeResponse> ToResponse(this List<RestrictionType> entities)
        {
            List<RestrictionTypeResponse> response = new();

            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }

        public static UserRestrictionResponse ToResponse(this UserRestriction entity) => new()
        {
            Id = entity.Id,
            CreationDate = entity.CreationDate,
            Description = entity.Description,
            ExpirationDate = entity.ExpirationDate,
            OwnerId = entity.OwnerId,
            Type = entity.Type?.ToResponse(),
            UserId = entity.UserId,
        };

        public static List<UserRestrictionResponse> ToResponse(this List<UserRestriction> entities)
        {
            List<UserRestrictionResponse> response = new();

            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }

        public static List<UserRestrictionResponse> ToResponse(this IEnumerable<UserRestriction> entities)
        {
            List<UserRestrictionResponse> response = new();

            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }

        public static UserRestrictionTemplate ToTemplate(this UserRestriction entity, bool isDeleted = false) => new()
        {
            Id = entity.Id,
            CreationDate = entity.CreationDate,
            Description = entity.Description,
            ExpirationDate = entity.ExpirationDate,
            OwnerId = entity.OwnerId,
            TypeId = entity.TypeId,
            UserId = entity.UserId,
            IsDeleted = isDeleted
        };

        public static UserRestriction ToEntity(this UserRestrictionRequest request, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : request.Id,
            CreationDate = request.CreationDate,
            Description = request.Description,
            ExpirationDate = request.ExpirationDate,
            OwnerId = request.OwnerId,
            TypeId = request.TypeId,
            UserId = request.UserId,
        };
    }
}
