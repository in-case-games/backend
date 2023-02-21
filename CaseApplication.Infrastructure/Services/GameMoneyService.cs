using CaseApplication.Domain.Entities.External;
using System.Net.Http.Json;
using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using CaseApplication.Domain.Entities.Internal;

namespace CaseApplication.Infrastructure.Services
{
    public class GameMoneyService
    {
        private readonly HttpClient _httpClient = new();
        private readonly RSAService _rsaService;
        private readonly IConfiguration _configuration;
        public GameMoneyService(
            RSAService rsaService,
            IConfiguration configuration)
        {
            _rsaService = rsaService;
            _configuration = configuration;
        }

        public async Task<InvoiceAnswerStatusGM?> GetInvoiceStatusInfo(int invoice)
        {
            string url = "https://paygate.gamemoney.com/invoice/status";
            InvoiceRequestStatusGM requestInvoice = new()
            {
                InvoiceId = invoice,
                ProjectId = int.Parse(_configuration["GameMoney:projectId"]!),
            };
            string hash = requestInvoice.ToString();
            requestInvoice.Signature = _rsaService.GenerateHMAC(Encoding.ASCII.GetBytes(hash));

            JsonContent json = JsonContent.Create(requestInvoice);
            HttpResponseMessage response = await _httpClient.PostAsync(url, json);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    response.StatusCode.ToString() +
                    response.RequestMessage! +
                    response.Headers +
                    response.ReasonPhrase! +
                    response.Content);
            }

            return await response.Content
                .ReadFromJsonAsync<InvoiceAnswerStatusGM>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
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
