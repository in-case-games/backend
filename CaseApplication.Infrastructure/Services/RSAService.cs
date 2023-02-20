using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using System.Text;

namespace CaseApplication.Infrastructure.Services
{
    public class RSAService
    {
        private readonly IConfiguration _configuration;

        public RSAService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateHMAC(byte[] hashOfDataToSign)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(_configuration["HMAC:Secret"]!);

            using HMACSHA256 hash = new(keyBytes);

            byte[] hashBytes = hash.ComputeHash(hashOfDataToSign);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            string pathPrivateKey = Path.Combine(Directory.GetCurrentDirectory(), "RsaKeys", _configuration["Tokens:PrivateKey"]!);
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

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            string pathPublicKey = Path.Combine(Directory.GetCurrentDirectory(), "RsaKeys", _configuration["Tokens:PublicKey"]!);
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
    }
}
