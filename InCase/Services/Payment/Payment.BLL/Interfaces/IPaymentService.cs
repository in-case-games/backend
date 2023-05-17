using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IPaymentService
    {
        public Task<UserPaymentsResponse> TopUpBalance(GameMoneyTopUpResponse request);
        public Task<decimal> GetPaymentBalance(string currency);
        public string GetHashOfDataForDeposit(Guid userId);
    }
}
