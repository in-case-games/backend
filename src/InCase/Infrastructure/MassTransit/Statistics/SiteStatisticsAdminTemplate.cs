namespace Infrastructure.MassTransit.Statistics
{
    public class SiteStatisticsAdminTemplate
    {
        public decimal FundsUsersInventories { get; set; } = 0;
        public decimal TotalReplenishedFunds { get; set; } = 0;
        public decimal ReturnedFunds { get; set; } = 0;
        public decimal RevenueLootBoxCommission { get; set; } = 0;
    }
}
