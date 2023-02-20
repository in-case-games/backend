namespace CaseApplication.Domain.Entities.External
{
    public class WithdrawItem
    {
        public Guid GameItemId { get; set; }
        public string? SteamTradePartner { get; set; }
        public string? SteamTradeToken { get; set; }
        public string? GenshinGuid { get; set; }
    }
}
