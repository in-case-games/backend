using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IGameMoneyService
    {
        public Task<PaymentBalanceResponse> GetBalanceAsync(string currency);
        public Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId);
        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId);
        public Task SendSuccess();
    }
}
