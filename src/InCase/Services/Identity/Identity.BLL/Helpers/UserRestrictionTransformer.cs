using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Helpers;
public static class UserRestrictionTransformer
{
    public static List<RestrictionTypeResponse> ToResponse(this List<RestrictionType> entities) =>
        entities.Select(entity => new RestrictionTypeResponse { Id = entity.Id, Name = entity.Name, }).ToList();

    public static UserRestrictionResponse ToResponse(this UserRestriction entity) => new()
    {
        Id = entity.Id,
        CreationDate = entity.CreationDate,
        Description = entity.Description,
        ExpirationDate = entity.ExpirationDate,
        OwnerId = entity.OwnerId,
        Type = entity.Type is null ? null : new RestrictionTypeResponse
        {
            Id = entity.Type.Id,
            Name = entity.Type.Name,
        },
        UserId = entity.UserId,
    };

    public static List<UserRestrictionResponse> ToResponse(this List<UserRestriction> entities) =>
        entities.Select(ToResponse).ToList();

    public static List<UserRestrictionResponse> ToResponse(this IEnumerable<UserRestriction> entities) =>
        entities.Select(ToResponse).ToList();

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

    public static UserRestriction ToEntity(this UserRestrictionRequest request, bool isNewGuid = false) => new()
    {
        Id = isNewGuid ? Guid.NewGuid() : request.Id,
        CreationDate = request.CreationDate,
        Description = request.Description,
        ExpirationDate = request.ExpirationDate,
        OwnerId = request.OwnerId,
        TypeId = request.TypeId,
        UserId = request.UserId,
    };
}