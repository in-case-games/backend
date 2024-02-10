using Payment.BLL.Models.Internal;

namespace Payment.BLL.Interfaces;
public interface IPaymentService
{
    public Task DoWorkManagerAsync(CancellationToken cancellationToken = default);
    public Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request, CancellationToken cancellationToken = default);
}