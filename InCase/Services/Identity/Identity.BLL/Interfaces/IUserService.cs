﻿using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAsync(Guid id);
        public Task<UserResponse> GetAsync(string login);
        public Task<UserResponse> CreateAsync(UserRequest request, bool IsNewGuid = false);
        public Task<UserResponse> UpdateLoginAsync(UserRequest request);
        public Task<UserResponse> DeleteAsync(Guid id);
    }
}
