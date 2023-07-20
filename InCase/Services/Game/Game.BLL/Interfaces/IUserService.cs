﻿using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(Guid id);
        public Task<UserResponse> CreateAsync(UserRequest request, bool IsNewGuid = false);
        public Task<UserResponse> DeleteAsync(Guid id);
    }
}