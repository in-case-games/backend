using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Resources.DAL.Entities;
public class GroupLootBox : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public LootBoxGroup? Group { get; set; }
}