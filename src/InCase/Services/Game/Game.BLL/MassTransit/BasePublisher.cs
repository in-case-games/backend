using MassTransit;

namespace Game.BLL.MassTransit;

public class BasePublisher(IBus bus)
{
    public async Task SendAsync<T>(T template, CancellationToken cancellationToken = default) where T : class
    {
        var endPoint = await bus.GetPublishSendEndpoint<T>();
        await endPoint.Send(template, cancellationToken);
    }
}