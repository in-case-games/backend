using System.Text.Json.Serialization;

namespace Resources.DAL.Entities
{
    public class PromocodeType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public Promocode? Promocode { get; set; }
    }
}
