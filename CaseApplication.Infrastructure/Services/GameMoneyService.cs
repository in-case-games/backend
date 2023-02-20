using CaseApplication.Domain.Entities.External;
using System.Net.Http.Json;
using System;
using System.Text;
using System.Text.Json;

namespace CaseApplication.Infrastructure.Services
{
    public class GameMoneyService
    {
        private readonly HttpClient _httpClient = new();
        private readonly RSAService _rsaService;
        public GameMoneyService(RSAService rsaService)
        {
            _rsaService = rsaService;
        }

        public async Task<ResponseInvoiceStatusPattern?> GetInvoiceStatusInfo(int invoice)
        {
            string url = "https://paygate.gamemoney.com/invoice/status";
            RequestInvoiceStatusPattern requestInvoice = new() { 
                Invoice = invoice,
                Project = 123123,
            };
            string hash = "invoice:[invoice];project:[project]";
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
                .ReadFromJsonAsync<ResponseInvoiceStatusPattern>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
