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
        private readonly IConfiguration _cfg;

        public EncryptorService(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public string GenerateHMAC(byte[] hashOfDataToSign)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(_cfg["GameMoney:HMACSecret"]!);

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
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "RsaKeys",
                _cfg["GameMoney:RSA:PublicKey"]!);
            using TextReader reader = new StringReader(File.ReadAllText(path));
            RsaKeyParameters param = (RsaKeyParameters)new PemReader(reader).ReadObject();

            RSACryptoServiceProvider rsa = new(2048);
            RSAParameters parms = new()
            {
                Modulus = param.Modulus.ToByteArrayUnsigned(),
                Exponent = param.Exponent.ToByteArrayUnsigned()
            };
            rsa.ImportParameters(parms);

            var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
            deformatter.SetHashAlgorithm("SHA256");

            return deformatter.VerifySignature(hashOfDataToSign, signature);
        }
    }
}
