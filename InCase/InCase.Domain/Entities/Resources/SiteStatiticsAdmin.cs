namespace InCase.Domain.Entities.Resources
{
    public class SiteStatiticsAdmin : BaseEntity
    {
        public decimal BalanceWithdrawn { get; set; } = 0;
        public decimal TotalReplenished { get; set; } = 0;
        public decimal TotalWithdrawn { get; set; } = 0;
        public decimal SentSites { get; set; } = 0;
    }
}
