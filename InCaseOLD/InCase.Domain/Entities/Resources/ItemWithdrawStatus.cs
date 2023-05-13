using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class ItemWithdrawStatus : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public UserHistoryWithdraw? HistoryWithdraw { get; set; }
    }
}
