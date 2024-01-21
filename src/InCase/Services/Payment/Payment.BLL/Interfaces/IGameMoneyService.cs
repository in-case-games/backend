using Payment.BLL.Models;

namespace Payment.BLL.Interfaces;

public interface IGameMoneyService
{
    public Task<PaymentBalanceResponse> GetBalanceAsync(string currency, CancellationToken cancellation = default);
    public Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId, CancellationToken cancellation = default);
    public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId);
    public Task SendSuccess(CancellationToken cancellation = default);
}