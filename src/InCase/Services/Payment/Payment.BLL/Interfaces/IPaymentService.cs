using Payment.BLL.Models.Internal;

namespace Payment.BLL.Interfaces;

public interface IPaymentService
{
    public Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationRequest request, CancellationToken cancellationToken = default);
    public Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request, CancellationToken cancellationToken = default);
}