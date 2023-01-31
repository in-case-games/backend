using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;

namespace CaseApplication.Api.Services
{
    public class EncryptorHelper
    {
        public string EncryptorPassword(string password, byte[] salt)
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

        public string GenerationSaltTo64Bytes()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(salt);
        }
    }
}
