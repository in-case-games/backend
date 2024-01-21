using System.Text.Json.Serialization;

namespace EmailSender.DAL.Entities;

public class User : BaseEntity
{
    public string? Email { get; set; }  

    [JsonIgnore]
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}