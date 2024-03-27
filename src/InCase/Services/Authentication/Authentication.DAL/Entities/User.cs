using System.ComponentModel.DataAnnotations;

namespace Authentication.DAL.Entities;
public class User : BaseEntity
{
    [MaxLength(50)]
    public string? Email { get; set; }
    [MaxLength(50)]
    public string? Login { get; set; }
    [MaxLength(200)]
    public string? PasswordHash { get; set; }
    [MaxLength(200)]
    public string? PasswordSalt { get; set; }

    public UserRestriction? Restriction { get; set; }
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}