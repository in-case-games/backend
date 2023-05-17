using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Payment.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Payment.BLL.Services
{
    public class EncryptorService : IEncryptorService
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

        public bool VerifySignatureRSA(IGameMoneyResponse response)
        {
            byte[] hash = Encoding.ASCII.GetBytes(response.ToString());
            byte[] signature = Encoding.ASCII.GetBytes(response!.SignatureRSA);

            return VerifySignatureRSA(hash, signature);
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
    }
}
