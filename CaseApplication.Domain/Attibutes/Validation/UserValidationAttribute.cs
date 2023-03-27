using CaseApplication.Domain.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CaseApplication.Domain.Attibutes.Validation
{
    public class UserValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            UserDto user = (UserDto)validationContext.ObjectInstance;

            if (!Regex.IsMatch(user.UserLogin!, @"^[\w]+$"))
                throw new Exception("The name should consist only of letters");
            if (user.UserLogin is null || user.UserLogin.Length < 2)
                throw new Exception("The name should consist biggest than 2 symbols");
            if (!Regex.Match(user.UserEmail!,
                "^((\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)\\s*[;,.]{0,1}\\s*)+$").Success)
                throw new Exception("The E-mail is invalid type");

            return ValidationResult.Success;
        }
    }
}
