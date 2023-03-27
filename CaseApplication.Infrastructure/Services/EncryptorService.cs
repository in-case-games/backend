using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using System.Text;

namespace CaseApplication.Infrastructure.Services
{
    public class EncryptorService
    {
        private readonly IConfiguration _configuration;

        public EncryptorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateHMAC(byte[] hashOfDataToSign)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(_configuration["GameMoney:HMACSecret"]!);

            using HMACSHA256 hash = new(keyBytes);

            byte[] hashBytes = hash.ComputeHash(hashOfDataToSign);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public byte[] SignDataRSA(byte[] hashOfDataToSign)
        {
            string pathPrivateKey = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "RsaKeys", 
                _configuration["GameMoney:RSA:PrivateKey"]!);
            using TextReader privateKeyTextReader = new StringReader(File.ReadAllText(pathPrivateKey));
            RsaKeyParameters privateKeyParam = (RsaKeyParameters)new PemReader(privateKeyTextReader).ReadObject();

            RSACryptoServiceProvider rsa = new(2048);
            RSAParameters parms = new()
            {
                Modulus = privateKeyParam.Modulus.ToByteArrayUnsigned(),
                Exponent = privateKeyParam.Exponent.ToByteArrayUnsigned()
            };
            rsa.PersistKeyInCsp = false;
            rsa.ImportParameters(parms);

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");

            return rsaFormatter.CreateSignature(hashOfDataToSign);
        }

        public bool VerifySignatureRSA(byte[] hashOfDataToSign, byte[] signature)
        {
            string pathPublicKey = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "RsaKeys", 
                _configuration["GameMoney:RSA:PublicKey"]!);
            using TextReader publicKeyTextReader = new StringReader(File.ReadAllText(pathPublicKey));
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

            RSACryptoServiceProvider rsa = new(2048);
            RSAParameters parms = new()
            {
                Modulus = publicKeyParam.Modulus.ToByteArrayUnsigned(),
                Exponent = publicKeyParam.Exponent.ToByteArrayUnsigned()
            };
            rsa.ImportParameters(parms);

            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");

            return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
        }

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
