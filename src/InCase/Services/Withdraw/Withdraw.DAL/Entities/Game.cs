using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Withdraw.DAL.Entities;
public class Game : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public IEnumerable<GameItem>? Items { get; set; }
    [JsonIgnore]
    public GameMarket? Market { get; set; }
}