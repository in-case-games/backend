using Payment.BLL.Models;
using Payment.DAL.Entities;

namespace Payment.BLL.Helpers
{
    public static class UserPaymentsTransformer
    {
        public static UserPaymentsResponse ToResponse(this UserPayments payment) =>
            new()
            {
                Id = payment.Id,
                InvoiceId = payment.InvoiceId,
                Status = payment.Status?.Name,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Date = payment.Date,
                Rate = payment.Rate,
            };

        public static List<UserPaymentsResponse> ToResponse(this List<UserPayments> payments)
        {
            List<UserPaymentsResponse> paymentsResponses = new();

            foreach (UserPayments payment in payments)
                paymentsResponses.Add(payment.ToResponse());

            return paymentsResponses;
        }
    }
}
