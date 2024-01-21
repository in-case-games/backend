namespace Infrastructure.MassTransit.User;

public class UserInventoryBackTemplate : BaseTemplate
{
    public decimal FixedCost { get; set; }

    public Guid UserId { get; set; }
}