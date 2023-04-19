namespace InCase.Domain.Entities.Payment
{
    public class DataWithdrawItem : PaymentEntity
    {
        public Guid ItemId { get; set; }
        public string? TradeUrl { get; set; }
    }
}
