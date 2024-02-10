using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Identity.DAL.Entities;
public class RestrictionType : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public UserRestriction? Restriction { get; set; }
}