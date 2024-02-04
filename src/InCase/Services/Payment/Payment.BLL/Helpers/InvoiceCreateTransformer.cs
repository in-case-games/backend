using Payment.BLL.Models.External;
using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers;

public static class InvoiceCreateTransformer
{
    public static InvoiceCreateRequest ToRequest(this InvoiceUrlRequest request) => 
        new()
        {
            Amount = request.Amount,
            Capture = true,
            Confirmation = new Confirmation { Type = "redirect", ReturnUrl = "https://localhost:3000/" },
            Metadata = new InvoiceCreateMetadata
            {
                UserId = request.User!.Id
            },
        };

    public static UserPayment ToEntity(this InvoiceCreateResponse response, User user, PaymentStatus status) => 
        new()
        {
            Id = Guid.NewGuid(),
            InvoiceId = response.Id,
            CreatedAt = response.CreatedAt.ToUniversalTime(),
            Currency = response.Amount!.Currency,
            Amount = response.Amount.Value,
            UserId = user.Id,
            StatusId = status.Id,
        };
}