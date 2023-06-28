using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Authentication.BLL.Services
{
    public static class EncryptorService
    {
        public static string GenerationHashSHA512(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return hashed;
        }

        public static byte[] GenerationSaltTo64Bytes()
        {
            return RandomNumberGenerator.GetBytes(64);
        }
    }
}
