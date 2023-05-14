using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class ItemWithdrawStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserHistoryWithdraw? HistoryWithdraw { get; set; }
    }
}
