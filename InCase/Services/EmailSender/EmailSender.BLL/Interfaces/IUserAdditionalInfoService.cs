using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id);
        public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid id);
        public Task<UserAdditionalInfoResponse> UpdateNotifyEmailAsync(Guid userId, bool isNotifyEmail);
    }
}
