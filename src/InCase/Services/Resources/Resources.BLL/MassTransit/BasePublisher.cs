using MassTransit;

namespace Resources.BLL.MassTransit
{
    public class BasePublisher
    {
        private readonly IBus _bus;

        public BasePublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendAsync<T>(T template, CancellationToken cancellationToken = default) where T : class
        {
            var endPoint = await _bus.GetPublishSendEndpoint<T>();
            await endPoint.Send(template, cancellationToken);
        }
    }
}
