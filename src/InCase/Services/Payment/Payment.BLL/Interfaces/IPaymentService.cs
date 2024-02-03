using Payment.BLL.Models.External.YooKassa;
using Payment.BLL.Models.Internal;

namespace Payment.BLL.Interfaces;

public interface IPaymentService
{
    public Task<bool> BindToEventAsync(BindingToEventRequest requestModel, CancellationToken cancellationToken = default);
    public Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationResponse request, CancellationToken cancellationToken = default);
    public Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request);
}