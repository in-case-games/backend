using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IUserPaymentsService
    {
        public Task<UserPaymentsResponse> GetByIdAsync(Guid id, Guid userId);
        public Task<List<UserPaymentsResponse>> GetAsync(int count);
        public Task<List<UserPaymentsResponse>> GetAsync(Guid userId, int count);
    }
}
