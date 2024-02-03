using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External;
using Payment.BLL.Models.External.YooKassa;
using Payment.BLL.Models.Internal;
using Payment.DAL.Data;
using Payment.DAL.Entities;
using System.Net;

namespace Payment.BLL.Services;

public class PaymentService(IResponseService responseService, ApplicationDbContext context) : IPaymentService
{
    public const string InvoiceCreateUri = "https://api.yookassa.ru/v3/payments";
    public const string WebhookCreateUri = "https://api.yookassa.ru/v3/webhooks";
    private IPAddress[] WebhookAddresses = new IPAddress[]
    {
        IPAddress.Parse("185.71.76.0/27"),
        IPAddress.Parse("185.71.77.0/27"),
        IPAddress.Parse("77.75.153.0/25"),
        IPAddress.Parse("77.75.156.11"),
        IPAddress.Parse("77.75.156.35"),
        IPAddress.Parse("77.75.154.128/25"),
        IPAddress.Parse("2a02:5180::/32")
    };

    public async Task<bool> BindToEventAsync(BindingToEventRequest requestModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!requestModel.IsValid())
            throw new BadRequestException("Неверная модель привязки");

        var response = await responseService
            .PostAsync<BindingToEventResponse, BindingToEventRequest>(WebhookCreateUri, requestModel, cancellationToken);

        return true;
    }

    public async Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationResponse response, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        PaymentStatus? paymentStatus = await context.PaymentStatuses
            .FirstOrDefaultAsync(x => x.Name!.ToLower() == response.Object!.Status);

        if (paymentStatus is null) throw new NotFoundException("Статус платежа не найден!");

        UserPayment? payment = await GetOrCreateUserPaymentAsync(response, paymentStatus);

        return new UserPaymentResponse()
        {
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            UserId = payment.UserId,
            Status = payment.Status
        };
    }

    public async Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request)
    {
        if (request.Amount is null) throw new BadRequestException("Заполните данные о сумме платежа");
        if (request.Amount.Value is <= 0 or > 100000) throw new BadRequestException("Сумма должна быть между 0 и 100000");

        request.Amount.Currency = request.Amount.Currency?.ToUpper();

        if (request.Amount.Currency != "RUB") throw new BadRequestException("Укажите валюту RUB");

        var response = await responseService
            .PostAsync<InvoiceCreateResponse, InvoiceCreateRequest>(InvoiceCreateUri, new InvoiceCreateRequest
            {
                Amount = request.Amount, 
                Capture = true, 
                Confirmation = new Confirmation
                {
                    Type = "redirect", 
                    ReturnUrl = "https://localhost:3000/"
                }, 
            });
    }

    private async Task<UserPayment> GetOrCreateUserPaymentAsync(InvoiceNotificationResponse response, PaymentStatus paymentStatus)
    {
        UserPayment? payment = await context.Payments
            .FirstOrDefaultAsync(x => x.InvoiceId == response.Object!.Id);

        if (payment is null)
        {
            payment = (await context.Payments.AddAsync(new UserPayment()
            {
                InvoiceId = response.Object!.Id,
                Amount = (decimal)response.Object?.Amount!.Value!,
                Currency = response.Object?.Amount!.Currency,
                UserId = (Guid)response.Object?.UserId!,
                StatusId = paymentStatus.Id,
                Status = paymentStatus
            })).Entity;

            await context.SaveChangesAsync();
        }

        return payment;
    }

    private bool ValidateIpAddress(IPAddress[] addresses, IPAddress currentAdress)
    {
        return addresses.Any(x => x == currentAdress);
    }
}