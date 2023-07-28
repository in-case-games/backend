using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IPaymentService
    {
        public Task<UserPaymentsResponse> TopUpBalanceAsync(GameMoneyTopUpResponse request);
        public Task<PaymentBalanceResponse> GetPaymentBalanceAsync(string currency);
        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId);
    }
}
