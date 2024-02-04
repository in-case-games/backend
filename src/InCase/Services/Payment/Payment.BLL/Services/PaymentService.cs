using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Microsoft.Extensions.Logging;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External;
using Payment.BLL.Models.External.YooKassa;
using Payment.BLL.Models.Internal;
using Payment.DAL.Data;
using Payment.DAL.Entities;
using System.Net;
using Payment.BLL.MassTransit;

namespace Payment.BLL.Services;

public class PaymentService(
    IResponseService responseService, 
    ILogger<PaymentService> logger, 
    ApplicationDbContext context,
    BasePublisher publisher) : IPaymentService
{
    public const string InvoiceCreateUri = "https://api.yookassa.ru/v3/payments";
    public const string WebhookCreateUri = "https://api.yookassa.ru/v3/webhooks";
    /*
     private readonly IPAddress[] _webhookAddresses =
    [
        IPAddress.Parse("185.71.76.0/27"),
        IPAddress.Parse("185.71.77.0/27"),
        IPAddress.Parse("77.75.153.0/25"),
        IPAddress.Parse("77.75.156.11"),
        IPAddress.Parse("77.75.156.35"),
        IPAddress.Parse("77.75.154.128/25"),
        IPAddress.Parse("2a02:5180::/32")
    ];
    */

    public async Task<bool> BindToEventAsync(BindingToEventRequest requestModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!requestModel.IsValid())
            throw new BadRequestException("Неверная модель привязки");

        await responseService
            .PostAsync<BindingToEventResponse, BindingToEventRequest>(WebhookCreateUri, requestModel, cancellationToken);

        return true;
    }

    public async Task ProcessingInvoiceNotificationAsync(InvoiceNotificationResponse response, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var findPayment = await context.Payments
            .AsNoTracking()
            .Include(p => p.Status)
            .FirstOrDefaultAsync(p => p.InvoiceId == response.Object!.Id, cancellationToken);

        var paymentStatus = await context.PaymentStatuses
            .FirstOrDefaultAsync(x => x.Name!.ToLower() == response.Object!.Status, cancellationToken);

        if(!ValidationService.IsValidInvoiceNotificationResponse(findPayment, response, paymentStatus, logger)) return;

        if (findPayment is null)
        {
            findPayment = (await context.Payments.AddAsync(new UserPayment
            {
                InvoiceId = response.Object!.Id,
                CreatedAt = DateTime.UtcNow,
                Amount = response.Object!.Amount!.Value!,
                Currency = response.Object!.Amount!.Currency,
                UserId = response.Object!.Metadata!.UserId,
                StatusId = paymentStatus!.Id,
            }, cancellationToken)).Entity;
        }
        else
        {
            findPayment.StatusId = paymentStatus!.Id;

            context.Entry(findPayment).Property(p => p.StatusId).IsModified = true;
        }

        await context.SaveChangesAsync(cancellationToken);

        if (paymentStatus.Name == "succeeded")
        { 
            await publisher.SendAsync(findPayment.ToTemplate(), cancellationToken);
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

        return new InvoiceUrlResponse { Url = response!.Confirmation!.ConfirmationUrl };
    }

    private static bool ValidateIpAddress(IEnumerable<IPAddress> addresses, IPAddress currentAddress) => 
        addresses.Any(x => x.Equals(currentAddress));
}