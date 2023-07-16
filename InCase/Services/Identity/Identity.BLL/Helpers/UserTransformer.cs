﻿using Identity.BLL.Models;
using Identity.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Identity.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User entity) => new()
        {
            Id = entity.Id,
            AdditionalInfo = entity.AdditionalInfo?.ToResponse(),
            Login = entity.Login,
            Restrictions = entity.Restrictions?.ToResponse(),
            OwnerRestrictions = entity.OwnerRestrictions?.ToResponse(),
        };

        public static List<UserResponse> ToResponse(this List<User> entities)
        {
            List<UserResponse> response = new();
            
            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }

        public static User ToEntity(this UserRequest request, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : request.Id,
            Login = request.Login,
        };

        public static UserRequest ToRequest(this UserTemplate template, bool IsNewGuid = false) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : template.Id,
            Login = template.Login,
        };
    }
}
