using EmailSender.BLL.Models;

namespace EmailSender.BLL.Interfaces;

public interface IUserAdditionalInfoService
{
    public Task<UserAdditionalInfoResponse> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<UserAdditionalInfoResponse> UpdateNotifyEmailAsync(Guid userId, bool isNotifyEmail, CancellationToken cancellationToken = default);
}