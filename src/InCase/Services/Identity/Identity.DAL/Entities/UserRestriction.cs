using System.Text.Json.Serialization;

namespace Identity.DAL.Entities;

public class UserRestriction : BaseEntity
{
    public DateTime CreationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string? Description { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public Guid? OwnerId { get; set; }
    [JsonIgnore]
    public Guid TypeId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public User? Owner { get; set; }
    [JsonIgnore]
    public RestrictionType? Type { get; set; }
}