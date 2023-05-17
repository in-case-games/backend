namespace Payment.BLL.Interfaces
{
    public interface IResponseService
    {
        public Task<IGameMoneyResponse?> ResponsePost(string uri, IGameMoneyRequest request);
    }
}
