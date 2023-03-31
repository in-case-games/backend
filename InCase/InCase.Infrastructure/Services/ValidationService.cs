using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Services
{
    public class ValidationService
    {
        public static bool IsValidUserPassword(in User user, string password)
        {
            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }
    }
}
