namespace Infrastructure.MassTransit.Withdraw
{
    public class UserInventoryTemplate : BaseTemplate
    {
        public DateTime Date { get; set; }
        public decimal FixedCost { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
    }
}
