using Payment.BLL.Models;

namespace Payment.BLL.Services
{
    public interface IUserPaymentsService
    {
        public Task<UserPaymentsResponse> GetByIdAsync(Guid id, Guid userId);
        public Task<List<UserPaymentsResponse>> GetAsync(Guid userId);
        public Task<List<UserPaymentsResponse>> GetAsync();
    }
}
