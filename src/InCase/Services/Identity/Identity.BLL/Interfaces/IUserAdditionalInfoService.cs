using Identity.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id);
        public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid userId);
        public Task<UserAdditionalInfoResponse> UpdateRoleAsync(Guid userId, Guid roleId);
        public Task<UserAdditionalInfoResponse> UpdateDeletionDateAsync(Guid userId, DateTime? deletionDate);
        public Task<UserAdditionalInfoResponse> UpdateImageAsync(Guid userId, IFormFile image);
    }
}