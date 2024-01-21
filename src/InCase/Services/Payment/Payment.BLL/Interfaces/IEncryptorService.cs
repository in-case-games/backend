namespace Payment.BLL.Interfaces;

public interface IEncryptorService
{
    public string GenerateHmac(byte[] hashOfDataToSign);
    public bool VerifySignatureRsa(IGameMoneyResponse response);
}