using Microsoft.Extensions.Logging;
using Payment.BLL.Exceptions;
using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;

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

    public static bool InvoiceCreateResponse<T>(InvoiceCreateResponse? response, ILogger<T> logger)
    {
        if (response?.Amount is null || string.IsNullOrEmpty(response?.Status)) 
        {
            logger.LogCritical($"Пришел не корректный счет - {response?.Id}{Environment.NewLine}" +
                               $"Сумма - {response?.Amount}{Environment.NewLine}" +
                               $"Статус - {response?.Status}{Environment.NewLine}" +
                               $"Ссылка для возврата - {response?.Confirmation?.ReturnUrl}{Environment.NewLine}" +
                               $"АккаунтId - {response?.Recipient?.AccountId}{Environment.NewLine}");
            return false;
        }

        return true;
    }
}