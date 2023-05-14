using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserHistoryWithdraw>? HistoryWithdraws { get; set; }
    }
}
