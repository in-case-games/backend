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

    public async Task DoWorkManagerAsync(CancellationToken cancellationToken)
    {
        var payments = await context.Payments
            .AsNoTracking()
            .Include(p => p.Status)
            .Where(p => p.ExpiresAt < DateTime.UtcNow && p.Status!.Name != "canceled" && p.Status.Name != "succeeded")
            .Take(10)
            .ToListAsync(cancellationToken);

        var canceledStatus = await context.PaymentStatuses.FirstAsync(ps => ps.Name == "canceled", cancellationToken);

        foreach (var payment in payments)
        {
            payment.StatusId = canceledStatus.Id;
            payment.ExpiresAt = null;
            context.Entry(payment).Property(p => p.StatusId).IsModified = true;
            context.Entry(payment).Property(p => p.ExpiresAt).IsModified = true;
            await context.SaveChangesAsync(cancellationToken);

            logger.LogTrace($"PaymentId - {payment.Id} истек по времени, изменили на статус отменён");
        }
    }

    public async Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request, CancellationToken cancellationToken = default)
    {
        ValidationService.InvoiceUrlRequest(request);

        request.Amount!.Currency = request.Amount.Currency?.ToUpper();

        responseService.FillYooKassaHttpClientHeaders();

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