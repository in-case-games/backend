using Microsoft.Extensions.Configuration;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Text;

namespace Payment.BLL.Services;

public class GameMoneyService(
    IResponseService responseService, 
    IEncryptorService rsaService, 
    IConfiguration cfg) : IGameMoneyService
{
    private const int NumberAttempts = 5;

    public async Task<PaymentBalanceResponse> GetBalanceAsync(string currency, CancellationToken cancellation = default)
    {
        var request = new GameMoneyBalanceRequest
        {
            Currency = currency,
            ProjectId = int.Parse(cfg["GameMoney:projectId"]!),
        };

        var hashBytes = Encoding.ASCII.GetBytes(request.ToString());
        request.SignatureHmac = rsaService.GenerateHmac(hashBytes);

        var i = 0;

        while(i < NumberAttempts)
        {
            try
            {
                var response = await responseService
                    .ResponsePostAsync(GameMoneyEndpoint.Balance, request, cancellation);

                if (!rsaService.VerifySignatureRsa(response!)) throw new ForbiddenException("Неверная подпись rsa");

                return new PaymentBalanceResponse
                { 
                    Balance = ((GameMoneyBalanceResponse)response!).ProjectBalance 
                };
            }
            catch (Exception)
            {
                i++;
            }

            await Task.Delay(200, cancellation);
        }

        throw new RequestTimeoutException("Сервис пополнения не отвечает");
    }

    public async Task SendSuccess(CancellationToken cancellation = default) => 
        await responseService.ResponsePostAsync(GameMoneyEndpoint.InvoiceInfo, 
            new GameMoneyNotifySuccessRequest { Success = true }, cancellation);

    public async Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId, CancellationToken cancellation = default)
    {
        var request = new GameMoneyInvoiceInfoRequest
        {
            ProjectId = cfg["GameMoney:projectId"],
            InvoiceId = invoiceId,
        };

        var hashBytes = Encoding.ASCII.GetBytes(request.ToString());
        request.SignatureHmac = rsaService.GenerateHmac(hashBytes);

        var i = 0;

        while(i < NumberAttempts)
        {
            try
            {
                var response = await responseService
                    .ResponsePostAsync(GameMoneyEndpoint.InvoiceInfo, request, cancellation);

                if (!rsaService.VerifySignatureRsa(response!)) throw new ForbiddenException("Неверная подпись rsa");

                return (GameMoneyInvoiceInfoResponse)response!;
            }
            catch(Exception)
            {
                i++;
            }

            await Task.Delay(200, cancellation);
        }

        throw new RequestTimeoutException("Сервис пополнения не отвечает");
    }

    public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) => 
        new()
        {
            Hmac = $"project:{cfg["GameMoney:projectId"]};" +
            $"user:{userId};" +
            $"currency:{cfg["GameMoney:currency"]};" +
            $"success_url:{cfg["GameMoney:url:success"]};" +
            $"fail_url:{cfg["GameMoney:url:fail"]};"
        };
}