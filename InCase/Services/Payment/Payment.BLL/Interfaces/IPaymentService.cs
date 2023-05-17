using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IPaymentService
    {
        public Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request);
        public Task DoWorkManagerAsync(CancellationToken cancellationToken);
        public Task<decimal> GetPaymentBalanceAsync(string currency);
        public string GetHashOfDataForDeposit(Guid userId);
    }
}
