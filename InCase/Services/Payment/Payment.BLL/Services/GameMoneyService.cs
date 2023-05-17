using InCase.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Payment.BLL.Exceptions;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using Payment.DAL.Entities;
using System.Text;

namespace Payment.BLL.Services
{
    public class GameMoneyService : IGameMoneyService
    {
        private static readonly int NumberAttempts = 5;
        private readonly ResponseService _responseService;
        private readonly EncryptorService _rsaService;
        private readonly IConfiguration _configuration;

        public GameMoneyService(
            ResponseService responseService, 
            EncryptorService rsaService, 
            IConfiguration configuration)
        {
            _responseService = responseService;
            _rsaService = rsaService;
            _configuration = configuration;
        }

        public async Task<decimal> GetBalance(string currency)
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
                    GameMoneyBalanceResponse? response = await _responseService
                        .ResponsePost<GameMoneyBalanceRequest, GameMoneyBalanceResponse>(
                        GameMoneyEndpoint.Balance, request);

                    if (!_rsaService.VerifySignatureRSA(response!))
                        throw new ForbiddenException("Неверная подпись rsa");

                    return response!.ProjectBalance;
                }
                catch (Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис пополнения не отвечает");
        }

        public async Task<GameMoneyInvoiceInfoResponse> GetInvoiceInfo(string invoiceId)
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
                    GameMoneyInvoiceInfoResponse? response = await _responseService
                        .ResponsePost<GameMoneyInvoiceInfoRequest, GameMoneyInvoiceInfoResponse>(
                        GameMoneyEndpoint.InvoiceInfo, request);

                    if (!_rsaService.VerifySignatureRSA(response!))
                        throw new ForbiddenException("Неверная подпись rsa");

                    return response!;
                }
                catch(Exception)
                {
                    i++;
                }
            }

            throw new RequestTimeoutException("Сервис пополнения не отвечает");
        }

        public string GetHashOfDataForDeposit(Guid userId) => 
            $"project:{_configuration["GameMoney:projectId"]};" +
            $"user:{userId};" +
            $"currency:{_configuration["GameMoney:currency"]};" +
            $"success_url:{_configuration["GameMoney:url:success"]};" +
            $"fail_url:{_configuration["GameMoney:url:fail"]};";
    }
}
