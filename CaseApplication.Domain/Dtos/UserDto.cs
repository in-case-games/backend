using CaseApplication.Domain.Attibutes.Validation;
using CaseApplication.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CaseApplication.Domain.Dtos
{
    [UserValidation]
    public class UserDto: BaseEntity
    {
        [Required]
        public string? UserLogin { get; set; }
        public string? UserImage { get; set; }
        [Required]
        public string? UserEmail { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
    }
}
