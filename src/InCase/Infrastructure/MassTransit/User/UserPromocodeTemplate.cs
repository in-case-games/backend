namespace Infrastructure.MassTransit.User;

public class UserPromocodeTemplate : BaseTemplate
{
    public decimal Discount { get; set; }

    public Guid UserId { get; set; }

    public PromocodeTypeTemplate? Type { get; set; }
}