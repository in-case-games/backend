using Payment.BLL.Models;

namespace Payment.BLL.Services
{
    public interface IUserPaymentsService
    {
        public Task<UserPaymentsResponse> GetByIdAsync(string id);
        public Task<List<UserPaymentsResponse>> GetAsync(Guid userId);
    }
}
