using MassTransit;

namespace Promocode.BLL.MassTransit;
public class BasePublisher(IBus bus)
{
    public async Task SendAsync<T>(T template, CancellationToken cancellation = default) where T : class
    {
        var endPoint = await bus.GetPublishSendEndpoint<T>();
        await endPoint.Send(template, cancellation);
    }
}