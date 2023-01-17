using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    public class UserDto : BaseEntity
    {
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
    }
}
