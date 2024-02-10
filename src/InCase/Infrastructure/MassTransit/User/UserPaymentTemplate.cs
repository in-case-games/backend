namespace Infrastructure.MassTransit.User;
public class UserPaymentTemplate : BaseTemplate
{
    public DateTime Date { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
}