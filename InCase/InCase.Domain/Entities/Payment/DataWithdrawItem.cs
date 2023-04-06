namespace InCase.Domain.Entities.Payment
{
    public class DataWithdrawItem : PaymentEntity
    {
        public Guid GameItemId { get; set; }
        public string? SteamTradePartner { get; set; }
        public string? SteamTradeToken { get; set; }
        public string? GenshinGuid { get; set; }
    }
}
