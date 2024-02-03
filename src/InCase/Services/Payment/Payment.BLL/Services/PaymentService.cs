using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;
using Payment.DAL.Data;

namespace Payment.BLL.Services;

public class PaymentService(
    IResponseService responseService, 
    ILogger<PaymentService> logger, 
    ApplicationDbContext context) : IPaymentService
{
    public const string InvoiceCreateUri = "https://api.yookassa.ru/v3/payments";

    public async Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request, CancellationToken cancellationToken = default)
    {
        ValidationService.InvoiceUrlRequest(request);

        request.Amount!.Currency = request.Amount.Currency?.ToUpper();

        var response = await responseService
            .PostAsync<InvoiceCreateResponse, InvoiceCreateRequest>(InvoiceCreateUri, request.ToRequest(), cancellationToken);

        logger.LogTrace($"POST invoice create response - {response}");

        ValidationService.InvoiceCreateResponse(response);

        var status = await context.PaymentStatuses.FirstAsync(ps => ps.Name == "pending", cancellationToken);

        var entity = response!.ToEntity(request.User!, status);

        await context.Payments.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new InvoiceUrlResponse
        {
            Url = response!.Confirmation!.ConfirmationUrl
        };
    }
}