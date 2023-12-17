using Infrastructure.MassTransit.User;
using Payment.BLL.Models;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers
{
    public static class UserPaymentsTransformer
    {
        public static UserPaymentsResponse ToResponse(this UserPayment payment) =>
            new()
            {
                Id = payment.Id,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Date = payment.Date,
                Rate = payment.Rate,
            };

        public static List<UserPaymentsResponse> ToResponse(this List<UserPayment> payments)
        {
            var paymentsResponses = new List<UserPaymentsResponse>();

            foreach (UserPayment payment in payments) paymentsResponses.Add(payment.ToResponse());

            return paymentsResponses;
        }
    }
}
