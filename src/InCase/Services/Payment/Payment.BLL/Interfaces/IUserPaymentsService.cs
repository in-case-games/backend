using Payment.BLL.Models.Internal;

namespace Payment.BLL.Interfaces;
public interface IUserPaymentsService
{
    public Task<UserPaymentResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation = default);
    public Task<List<UserPaymentResponse>> GetAsync(int count, CancellationToken cancellation = default);
    public Task<List<UserPaymentResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default);
}