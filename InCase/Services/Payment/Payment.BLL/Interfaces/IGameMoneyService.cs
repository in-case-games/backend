using Payment.BLL.Models;

namespace Payment.BLL.Interfaces
{
    public interface IGameMoneyService
    {
        public Task<decimal> GetBalance(string currency);
        public Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfo(string invoiceId);
        public string GetHashOfDataForDeposit(Guid userId);
    }
}
