using InCase.Infrastructure.Services;
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
        private readonly IConfiguration _configuration;

        public GameMoneyService(
            IResponseService responseService, 
            IEncryptorService rsaService, 
            IConfiguration configuration)
        {
            _responseService = responseService;
            _rsaService = rsaService;
            _configuration = configuration;
        }

        public async Task<PaymentBalanceResponse> GetBalanceAsync(string currency)
        {
            GameMoneyBalanceRequest request = new()
            {
                Currency = currency,
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
            };

            byte[] hashBytes = Encoding.ASCII.GetBytes(request.ToString());
            request.SignatureHMAC = _rsaService.GenerateHMAC(hashBytes);

            int i = 0;

            while(i < NumberAttempts)
            {
                try
                {
                    IGameMoneyResponse? response = await _responseService
                        .ResponsePostAsync(GameMoneyEndpoint.Balance, request);

                    if (!_rsaService.VerifySignatureRSA(response!))
                        throw new ForbiddenException("Неверная подпись rsa");

                    return new() { 
                        Balance = ((GameMoneyBalanceResponse)response!).ProjectBalance 
                    };
                }
                catch (Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис пополнения не отвечает");
        }

        public async Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfoAsync(string invoiceId)
        {
            GameMoneyInvoiceInfoRequest request = new()
            {
                ProjectId = _configuration["GameMoney:projectId"],
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
                        .ResponsePostAsync(GameMoneyEndpoint.InvoiceInfo, request);

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
            HMAC = $"project:{_configuration["GameMoney:projectId"]};" +
            $"user:{userId};" +
            $"currency:{_configuration["GameMoney:currency"]};" +
            $"success_url:{_configuration["GameMoney:url:success"]};" +
            $"fail_url:{_configuration["GameMoney:url:fail"]};"
        };
    }
}
