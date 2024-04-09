namespace Game.BLL.Interfaces;
public interface IBasePublisher
{
    public Task SendAsync<T>(T template, CancellationToken cancellationToken = default) where T : class;
}