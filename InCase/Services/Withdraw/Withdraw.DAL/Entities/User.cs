using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserInventory>? Inventories { get; set; }
    }
}
