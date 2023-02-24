using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using CaseApplication.Domain.Entities.Payment;
using CaseApplication.Domain.Endpoints;

namespace CaseApplication.Infrastructure.Services
{
    public class GameMoneyService
    {
        private readonly HttpClient _httpClient = new();
        private readonly EncryptorService _rsaService;
        private readonly IConfiguration _configuration;
        public GameMoneyService(
            EncryptorService rsaService,
            IConfiguration configuration)
        {
            _rsaService = rsaService;
            _configuration = configuration;
        }

        public async Task<ResponseBalanceGM?> GetBalanceInfo(string currency)
        {
            RequestBalanceGM requestBalanceInfo = new()
            {
                Currency = currency,
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
            };

            string hash = requestBalanceInfo.ToString();
            requestBalanceInfo.SignatureHMAC = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            return await PaymentResponse<ResponseBalanceGM, RequestBalanceGM>
                (PaygateEndpoint.Balance, requestBalanceInfo);
        }

        //TODO
        public async Task<ResponseInsertGM?> TransferMoneyToTradeMarket(decimal ammount)
        {
            RequestInsertGM requestInsertGM = new()
            {
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
                PaymentId = new Guid(),
                UserId = new Guid(),
                UserIp = "1.0.1",
                PaymentAmount = ammount
            };

            byte[] hash = Encoding.ASCII.GetBytes(requestInsertGM.ToString());
            requestInsertGM.SignatureRSA = Encoding.ASCII.GetString(_rsaService.SignDataRSA(hash));

            return await PaymentResponse<ResponseInsertGM, RequestInsertGM>
                (PaygateEndpoint.Transfer, requestInsertGM);
        }

        public async Task<ResponseInvoiceStatusGM?> GetInvoiceStatusInfo(int invoice)
        {
            RequestInvoiceStatusGM requestInvoice = new()
            {
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
                InvoiceId = invoice,
            };

            requestInvoice.SignatureHMAC = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(requestInvoice.ToString()));

            return await PaymentResponse<ResponseInvoiceStatusGM, RequestInvoiceStatusGM>
                (PaygateEndpoint.InvoiceInfo, requestInvoice);
        }
        public async Task<T> PaymentResponse<T, O>(string url, O entity) where T: PaymentEntity
        {
            JsonContent json = JsonContent.Create(entity);
            HttpResponseMessage response = await _httpClient.PostAsync(url, json);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString() + response.RequestMessage!);

            T? responseEntity =  await response.Content
                .ReadFromJsonAsync<T>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return responseEntity!;
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
