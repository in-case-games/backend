using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserDto : BaseEntity
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public User Convert() => new()
        {
            Login = Login,
            Email = Email,
            PasswordHash = PasswordHash,
            PasswordSalt = PasswordSalt
        };
    }
}
