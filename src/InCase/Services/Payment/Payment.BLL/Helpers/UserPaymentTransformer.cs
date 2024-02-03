using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers;

public static class UserPaymentTransformer
{
    public static UserPaymentResponse ToResponse(this UserPayment payment) =>
        new()
        {
            Id = payment.Id,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Date = payment.Date,
            UserId = payment.UserId,
            Status = payment.Status
        };

    public static List<UserPaymentResponse> ToResponse(this List<UserPayment> payments) =>
        payments.Select(ToResponse).ToList();
}