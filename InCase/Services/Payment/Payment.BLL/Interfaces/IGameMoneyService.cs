using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IGameMoneyService
    {
        public Task<decimal> GetBalanceAsync(string currency);
        public Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId);
        public string GetHashOfDataForDeposit(Guid userId);
    }
}
