namespace Withdraw.BLL.Interfaces
{
    public interface IResponseService
    {
        public Task<T?> GetAsync<T>(string uri, CancellationToken cancellation = default);
    }
}
