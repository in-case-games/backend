namespace Test.Domain.Entities
{
    public class SiteStatiticsAdmin : BaseEntity
    {
        public decimal BalanceWithdrawn { get; set; } = 0;
        public decimal TotalReplenished { get; set; } = 0;
        public decimal TotalWithdrawn { get; set; } = 0;
    }
}
