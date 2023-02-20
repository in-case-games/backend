namespace CaseApplication.Domain.Entities.External
{
    public class WithdrawItem
    {
        public Guid GameItemId { get; set; }
        public string? SteamTradeUrl { get; set; }
        public string? GenshinGuid { get; set; }
    }
}
