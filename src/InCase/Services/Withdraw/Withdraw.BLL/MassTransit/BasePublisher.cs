using MassTransit;

namespace Withdraw.BLL.MassTransit;
public class BasePublisher(IBus bus)
{
    public async Task SendAsync<T>(T template, CancellationToken token = default) where T : class
    {
        var endPoint = await bus.GetPublishSendEndpoint<T>();
        await endPoint.Send(template, token);
    }
}