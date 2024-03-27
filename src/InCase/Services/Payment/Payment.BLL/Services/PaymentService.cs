using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;
using Payment.DAL.Data;
using Payment.BLL.MassTransit;
using Infrastructure.MassTransit.User;
using Payment.BLL.Exceptions;

namespace Payment.BLL.Services;
public class PaymentService(
    IResponseService responseService, 
    ILogger<PaymentService> logger, 
    ApplicationDbContext context,
    BasePublisher publisher) : IPaymentService
{
    public const string InvoiceUri = "https://api.yookassa.ru/v3/payments";

    public async Task DoWorkManagerAsync(CancellationToken cancellationToken = default)
    {
        var payments = await context.UserPayments
            .AsNoTracking()
            .Include(p => p.Status)
            .Where(p => p.Status!.Name != "canceled" && 
                                  p.Status!.Name != "succeeded" &&
                                  p.UpdateTo <= DateTime.UtcNow)
            .OrderByDescending(p => p.UpdateTo)
            .Take(3)
            .ToListAsync(cancellationToken);

        var statuses = await context.PaymentStatuses.ToListAsync(cancellationToken);

        foreach (var payment in payments)
        {
            payment.UpdateTo = DateTime.UtcNow.AddMinutes(5);
            context.Entry(payment).Property(p => p.UpdateTo).IsModified = true;
            await context.SaveChangesAsync(cancellationToken);

            responseService.FillYooKassaHttpClientHeaders();
            var response = await responseService
                .GetAsync<InvoiceCreateResponse>(InvoiceUri + $"/{payment.InvoiceId}", cancellationToken);

            logger.LogTrace($"Получил информацию о платеже - {response}");

            if (!ValidationService.InvoiceCreateResponse(response, logger)) continue;

            var statusName = response?.Status?.Contains("waiting", StringComparison.OrdinalIgnoreCase) is false ? 
                                    response.Status?.ToLower() : 
                                    "waiting";

            var status = statuses.FirstOrDefault(s => s.Name?.ToLower() == statusName);

            if (status is null)
            {
                logger.LogCritical($"{response?.Id} - неизвестный статус {response?.Status}");
                continue;
            }
            if (payment.StatusId == status.Id)
            {
                logger.LogTrace($"{response?.Id} - статус уже отработан {response?.Status}");
                continue;
            }

            payment.StatusId = status.Id;
            context.Entry(payment).Property(p => p.StatusId).IsModified = true;
            await context.SaveChangesAsync(cancellationToken);

            if (status.Name != "succeeded") continue;

            var promo = await context.UserPromoCodes
                .FirstOrDefaultAsync(up => up.UserId == payment.UserId, cancellationToken);

            if (promo is not null)
            {
                promo.Discount = promo.Discount >= 0.99M ? 1 : promo.Discount;
                promo.Discount += 1;
                payment.Amount *= promo.Discount;

                context.UserPromoCodes.Remove(promo);
                await context.SaveChangesAsync(cancellationToken);

                await publisher.SendAsync(new UserPromoCodeBackTemplate
                {
                    Id = promo.Id,
                }, cancellationToken);
            }

            await publisher.SendAsync(payment.ToTemplate(), cancellationToken);
            await publisher.SendAsync(new SiteStatisticsAdminTemplate
            {
                TotalReplenishedFunds = payment.Amount
            }, cancellationToken);
        }
    }

    public async Task<InvoiceUrlResponse> CreateInvoiceUrlAsync(InvoiceUrlRequest request, CancellationToken cancellationToken = default)
    {
        ValidationService.InvoiceUrlRequest(request);

        request.Amount!.Currency = request.Amount.Currency?.ToUpper();

        responseService.FillYooKassaHttpClientHeaders();
        var response = await responseService
            .PostAsync<InvoiceCreateResponse, InvoiceCreateRequest>(InvoiceUri, request.ToRequest(), cancellationToken);

        logger.LogTrace($"Получил информацию о создании платежа - {response}");

        if(!ValidationService.InvoiceCreateResponse(response, logger)) throw new UnknownException("Внутренняя ошибка");

        var status = await context.PaymentStatuses.FirstAsync(ps => ps.Name == "pending", cancellationToken);
        var entity = response!.ToEntity(request.User!.Id, status.Id);

        await context.UserPayments.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new InvoiceUrlResponse { Url = response!.Confirmation!.ConfirmationUrl };
    }
}