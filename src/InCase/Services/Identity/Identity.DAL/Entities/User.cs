using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Identity.DAL.Entities;
public class User : BaseEntity
{
    [MaxLength(50)]
    public string? Login { get; set; }

    [JsonIgnore]
    public IEnumerable<UserRestriction>? Restrictions { get; set; }
    [JsonIgnore]
    public IEnumerable<UserRestriction>? OwnerRestrictions { get; set; }
    [JsonIgnore]
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}