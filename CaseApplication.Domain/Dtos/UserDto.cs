using CaseApplication.Domain.Attibutes.Validation;
using CaseApplication.Domain.Entities;

namespace CaseApplication.Domain.Dtos
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
