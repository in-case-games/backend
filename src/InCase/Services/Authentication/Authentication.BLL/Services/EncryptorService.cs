using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Authentication.BLL.Services;

public static class EncryptorService
{
    public static string GenerationHashSha512(string password, byte[] salt) => 
        Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
            ));

    public static byte[] GenerationSaltTo64Bytes() => RandomNumberGenerator.GetBytes(64);
}