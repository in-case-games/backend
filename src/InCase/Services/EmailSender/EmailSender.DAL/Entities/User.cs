using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmailSender.DAL.Entities;
public class User : BaseEntity
{
    [MaxLength(50)]
    public string? Email { get; set; }  

    [JsonIgnore]
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}