using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IUserPromocodeService
    {
        public Task<UserPromocodeResponse> GetAsync(Guid id);
        public Task<UserPromocodeResponse> CreateAsync(UserPromocodeRequest request, bool isNewGuid = false);
        public Task<UserPromocodeResponse> UpdateAsync(UserPromocodeRequest request);
    }
}
