namespace Payment.BLL.Interfaces
{
    public interface IEncryptorService
    {
        public string GenerateHMAC(byte[] hashOfDataToSign);
        public bool VerifySignatureRSA(IGameMoneyResponse response);
        public bool VerifySignatureRSA(byte[] hashOfDataToSign, byte[] signature);
    }
}
