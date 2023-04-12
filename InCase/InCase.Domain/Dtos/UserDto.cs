using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class UserDto : BaseEntity
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Ip { get; set; } = "";
        public string? Platform { get; set; } = "";

        public User Convert(bool IsNewGuid = true) => new()
        {
            Id = IsNewGuid ? Guid.NewGuid() : Id,
            Login = Login,
            Email = Email
        };
    }
}
