namespace Payment.BLL.Models
{
    public class UserPaymentsResponse
    {
        public string? Id { get; set; }
        public string? InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public string? Status { get; set; }
    }
}
