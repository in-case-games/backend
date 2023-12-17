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
            using var hash = new HMACSHA256(Encoding.ASCII.GetBytes(_cfg["GameMoney:HMACSecret"]!));

            var hashBytes = hash.ComputeHash(hashOfDataToSign);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public bool VerifySignatureRSA(IGameMoneyResponse response)
        {
            var hash = Encoding.ASCII.GetBytes(response.ToString());
            var signature = Encoding.ASCII.GetBytes(response!.SignatureRSA);

            return VerifySignatureRSA(hash, signature);
        }

        public bool VerifySignatureRSA(byte[] hashOfDataToSign, byte[] signature)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "RsaKeys", _cfg["GameMoney:RSA:PublicKey"]!);

            using var reader = new StringReader(File.ReadAllText(path));

            var param = (RsaKeyParameters)new PemReader(reader).ReadObject();

            var rsa = new RSACryptoServiceProvider(2048);
            var parms = new RSAParameters()
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
