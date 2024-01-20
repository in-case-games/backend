using MassTransit;

namespace Payment.BLL.MassTransit
{
    public class BasePublisher(IPublishEndpointProvider bus)
    {
        public async Task SendAsync<T>(T template, CancellationToken cancellation = default) where T : class
        {
            var endPoint = await bus.GetPublishSendEndpoint<T>();
            await endPoint.Send(template, cancellation);
        }
    }
}
