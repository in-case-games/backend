using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserPromocode>? HistoriesPromocodes { get; set; }
    }
}
