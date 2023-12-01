using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IPaymentService
    {
        public Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request, CancellationToken cancellation = default);
        public Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency, CancellationToken cancellation = default);
        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId);
    }
}
