using System.Text.Json.Serialization;

namespace InCase.Domain.Entities.Resources
{
    public class PromocodeType : BaseEntity
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public Promocode? Promocode { get; set; }
    }
}
