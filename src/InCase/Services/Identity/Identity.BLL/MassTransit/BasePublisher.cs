using MassTransit;

namespace Identity.BLL.MassTransit
{
    public class BasePublisher
    {
        private readonly IBus _bus;

        public BasePublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendAsync<T>(T template, CancellationToken cancellation = default) where T : class
        {
            var endPoint = await _bus.GetPublishSendEndpoint<T>();
            await endPoint.Send(template, cancellation);
        }
    }
}
