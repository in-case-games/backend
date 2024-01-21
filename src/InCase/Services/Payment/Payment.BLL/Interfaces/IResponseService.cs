namespace Payment.BLL.Interfaces;

public interface IResponseService
{
    public Task<IGameMoneyResponse?> ResponsePostAsync(string uri, IGameMoneyRequest request, CancellationToken cancellation = default);
}