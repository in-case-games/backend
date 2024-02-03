using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;

namespace Payment.BLL.Services;

public class PaymentService(IResponseService responseService) : IPaymentService
{
    public const string InvoiceCreateUri = "https://api.yookassa.ru/v3/payments";

    public async Task<UserPaymentResponse> ProcessingInvoiceNotificationAsync(InvoiceNotificationRequest request)
    {
        throw new NotImplementedException();
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
}