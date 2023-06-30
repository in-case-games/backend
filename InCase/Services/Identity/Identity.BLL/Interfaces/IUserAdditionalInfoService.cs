using Identity.BLL.Models;

namespace Identity.BLL.Interfaces
{
    public interface IUserAdditionalInfoService
    {
        public Task<UserAdditionalInfoResponse> GetAsync(Guid id, CancellationToken cancellationToken = default);
        public Task UpdateAsync(UserAdditionalInfoRequest request, CancellationToken cancellationToken = default);
    }
}