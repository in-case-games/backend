using Microsoft.Extensions.Logging;
using Payment.BLL.Exceptions;
using Payment.BLL.Models.External;
using Payment.BLL.Models.External.YooKassa;
using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.BLL.Services;

public static class ValidationService
{
    public static void InvoiceUrlRequest(InvoiceUrlRequest? request)
    {
        if (request?.Amount is null) throw new BadRequestException("Заполните данные о платеже");
        if (request.User is null) throw new UnknownException("Внутренняя ошибка");
        if (request.Amount.Value is <= 0 or > 100000) throw new BadRequestException("Сумма должна быть между 0 и 100000");
        if (request.Amount.Currency?.ToUpper() != "RUB") throw new BadRequestException("Укажите валюту RUB");
    }

    public static void InvoiceCreateResponse(InvoiceCreateResponse? response)
    {
        if (response?.Amount is null || 
            string.IsNullOrEmpty(response.Status) ||
            response.Confirmation is null ||
            response.Recipient is null) throw new UnknownException("Внутренняя ошибка");
    }

    public static bool IsValidInvoiceNotificationResponse(UserPayment? payment, 
        InvoiceNotificationResponse notify, PaymentStatus? status, ILogger<PaymentService> logger)
    {
        if (payment is null && notify.Object!.Metadata is null)
        {
            logger.LogCritical($"{notify.Object.Id} - не был передан объект metadata");
            return false;
        }
        if (payment is not null && payment.Status!.Name == "succeeded")
        {
            logger.LogCritical($"{notify.Object!.Id} - уже был отработан платежной системой");
            return false;
        }
        if (status is not null) return true;

        logger.LogCritical($"{notify.Object!.Id} - status {notify.Object!.Status} не найден");
        return false;
    }
}