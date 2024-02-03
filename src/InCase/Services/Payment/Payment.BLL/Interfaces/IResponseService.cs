namespace Payment.BLL.Interfaces;

public interface IResponseService
{
    public Task<T?> GetAsync<T>(string uri, CancellationToken cancellationToken = default);
    public Task<T?> PostAsync<T, TK>(string uri, TK body, CancellationToken cancellationToken = default);
}