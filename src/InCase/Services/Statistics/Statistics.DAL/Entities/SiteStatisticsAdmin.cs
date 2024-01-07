namespace Statistics.DAL.Entities
{
    public class SiteStatisticsAdmin : BaseEntity
    {
        public decimal FundsUsersInventories { get; set; } = 0;
        public decimal TotalReplenishedFunds { get; set; } = 0;
        public decimal ReturnedFunds { get; set; } = 0;
        public decimal RevenueLootBoxCommission { get; set; } = 0;
    }
}
