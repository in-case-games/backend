using Infrastructure.MassTransit.User;
using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers;
public static class UserPaymentTransformer
{
    private const int RubToInCoin = 7;

    public static UserPaymentResponse ToResponse(this UserPayment payment) =>
        new()
        {
            Id = payment.Id,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Date = payment.CreatedAt,
            UserId = payment.UserId,
            Status = payment.Status
        };

    public static List<UserPaymentResponse> ToResponse(this List<UserPayment> payments) =>
        payments.Select(ToResponse).ToList();

    public static UserPaymentTemplate ToTemplate(this UserPayment payment) =>
        new()
        {
            Id = payment.Id,
            Date = payment.CreatedAt,
            Currency = payment.Currency,
            Amount = payment.Amount * RubToInCoin,
            UserId = payment.UserId
        };
}