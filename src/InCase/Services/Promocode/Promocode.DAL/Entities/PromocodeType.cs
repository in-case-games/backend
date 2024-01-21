using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities;

public class PromocodeType : BaseEntity
{
    public string? Name { get; set; }

    [JsonIgnore]
    public PromocodeEntity? Promocode { get; set; }
}