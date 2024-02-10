namespace Infrastructure.MassTransit.User;
public class UserPromoCodeTemplate : BaseTemplate
{
    public decimal Discount { get; set; }

    public Guid UserId { get; set; }

    public PromoCodeTypeTemplate? Type { get; set; }
}