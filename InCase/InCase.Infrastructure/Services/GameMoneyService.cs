using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using InCase.Domain.Entities.Payment;
using InCase.Domain.Endpoints;

namespace InCase.Infrastructure.Services
{
    public class GameMoneyService
    {
        private readonly ResponseService _responseService;
        private readonly EncryptorService _rsaService;
        private readonly IConfiguration _configuration;
        public GameMoneyService(
            EncryptorService rsaService,
            ResponseService responseService,
            IConfiguration configuration)
        {
            _rsaService = rsaService;
            _responseService = responseService;
            _configuration = configuration;
        }

        public async Task<ResponseBalanceGM?> GetBalance(string currency)
        {
            RequestBalanceGM request = new()
            {
                Currency = currency,
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
            };

            string hash = request.ToString();
            request.SignatureHMAC = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return await _responseService
                .ResponsePost<RequestBalanceGM, ResponseBalanceGM>(PaygateEndpoint.Balance, request);
        }

        //TODO
        public async Task<ResponseInsertGM?> TransferMoneyToTradeMarket(decimal ammount)
        {
            RequestInsertGM request = new()
            {
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
                PaymentId = new Guid(),
                UserId = new Guid(),
                UserIp = "1.0.1",
                PaymentAmount = ammount
            };

            byte[] hash = Encoding.ASCII.GetBytes(request.ToString());
            request.SignatureRSA = Encoding.ASCII.GetString(_rsaService.SignDataRSA(hash));

            return await _responseService
                .ResponsePost<RequestInsertGM, ResponseInsertGM>(PaygateEndpoint.Transfer, request);
        }

        public async Task<ResponseInvoiceStatusGM?> GetInvoiceStatusInfo(string invoice)
        {
            RequestInvoiceStatusGM request = new()
            {
                ProjectId = _configuration["GameMoney:projectId"],
                InvoiceId = invoice,
            };

            request.SignatureHMAC = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(request.ToString()));

            return await _responseService
                .ResponsePost<RequestInvoiceStatusGM, ResponseInvoiceStatusGM>(PaygateEndpoint.InvoiceInfo, request);
        }

        public string CreateHashOfDataForDeposit(Guid userId)
        {
            return $"project:{_configuration["GameMoney:projectId"]};" +
                   $"user:{userId};" +
                   $"currency:{_configuration["GameMoney:currency"]};" +
                   $"success_url:{_configuration["GameMoney:url:success"]};" +
                   $"fail_url:{_configuration["GameMoney:url:fail"]};";
        }
    }
}
