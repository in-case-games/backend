using CaseApplication.DomainLayer.Attibutes.Validation;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.DomainLayer.Dtos
{
    [UserValidation]
    public class UserDto: BaseEntity
    {
        public string? UserLogin { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
    }
}
