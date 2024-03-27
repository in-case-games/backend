using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers;
public static class InvoiceCreateTransformer
{
    private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    public static InvoiceCreateRequest ToRequest(this InvoiceUrlRequest request) => 
        new()
        {
            Amount = request.Amount,
            Capture = true,
            Confirmation = new Confirmation { 
                Type = "redirect", 
                ReturnUrl = Env == "Production" ? "https://in-case.games" : "http://localhost:3000/" 
            },
            Metadata = new InvoiceCreateMetadata
            {
                UserId = request.User!.Id
            },
        };

    public static UserPayment ToEntity(this InvoiceCreateResponse response, Guid userId, Guid statusId) => 
        new()
        {
            Id = Guid.NewGuid(),
            InvoiceId = response.Id,
            CreatedAt = response.CreatedAt.ToUniversalTime(),
            UpdateTo = DateTime.UtcNow.AddSeconds(30),
            Currency = response.Amount!.Currency,
            Amount = response.Amount.Value,
            UserId = userId,
            StatusId = statusId,
        };
}