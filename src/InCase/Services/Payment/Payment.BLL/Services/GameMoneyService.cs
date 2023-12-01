using Microsoft.Extensions.Configuration;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using System.Text;

namespace Payment.BLL.Services
{
    public class GameMoneyService : IGameMoneyService
    {
        private static readonly int NumberAttempts = 5;
        private readonly IResponseService _responseService;
        private readonly IEncryptorService _rsaService;
        private readonly IConfiguration _cfg;

        public GameMoneyService(
            IResponseService responseService, 
            IEncryptorService rsaService, 
            IConfiguration cfg)
        {
            _responseService = responseService;
            _rsaService = rsaService;
            _cfg = cfg;
        }

        public async Task<PaymentBalanceResponse> GetBalanceAsync(string currency, CancellationToken cancellation = default)
        {
            GameMoneyBalanceRequest request = new()
            {
                Currency = currency,
                ProjectId = int.Parse(_cfg["GameMoney:projectId"]!),
            };

            byte[] hashBytes = Encoding.ASCII.GetBytes(request.ToString());
            request.SignatureHMAC = _rsaService.GenerateHMAC(hashBytes);

            int i = 0;

            while(i < NumberAttempts)
            {
                try
                {
                    IGameMoneyResponse? response = await _responseService
                        .ResponsePostAsync(GameMoneyEndpoint.Balance, request, cancellation);

                    if (!_rsaService.VerifySignatureRSA(response!))
                        throw new ForbiddenException("Неверная подпись rsa");

                    return new() { Balance = ((GameMoneyBalanceResponse)response!).ProjectBalance };
                }
                catch (Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис пополнения не отвечает");
        }

        public async Task SendSuccess(CancellationToken cancellation = default)
        {
            GameMoneyNotifySuccessRequest request = new GameMoneyNotifySuccessRequest();
            request.Success = true;
            IGameMoneyResponse? response = await _responseService
                        .ResponsePostAsync(GameMoneyEndpoint.InvoiceInfo, request, cancellation);
        }

        public async Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId, CancellationToken cancellation = default)
        {
            GameMoneyInvoiceInfoRequest request = new()
            {
                ProjectId = _cfg["GameMoney:projectId"],
                InvoiceId = invoiceId,
            };

            byte[] hashBytes = Encoding.ASCII.GetBytes(request.ToString());
            request.SignatureHMAC = _rsaService.GenerateHMAC(hashBytes);

            int i = 0;

            while(i < NumberAttempts)
            {
                try
                {
                    IGameMoneyResponse? response = await _responseService
                        .ResponsePostAsync(GameMoneyEndpoint.InvoiceInfo, request, cancellation);

                    if (!_rsaService.VerifySignatureRSA(response!))
                        throw new ForbiddenException("Неверная подпись rsa");

                    return (GameMoneyInvoiceInfoResponse)response!;
                }
                catch(Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис пополнения не отвечает");
        }

        public HashOfDataForDepositResponse GetHashOfDataForDeposit(Guid userId) => new()
        {
            HMAC = $"project:{_cfg["GameMoney:projectId"]};" +
            $"user:{userId};" +
            $"currency:{_cfg["GameMoney:currency"]};" +
            $"success_url:{_cfg["GameMoney:url:success"]};" +
            $"fail_url:{_cfg["GameMoney:url:fail"]};"
        };
    }
}
