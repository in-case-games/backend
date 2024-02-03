using Payment.BLL.Models.Internal;

namespace Payment.BLL.Interfaces;

public interface IPaymentService
{
    public Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationRequest request);
    public Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request);
}