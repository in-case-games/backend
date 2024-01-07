using MassTransit;

namespace Withdraw.BLL.MassTransit
{
    public class BasePublisher
    {
        private readonly IBus _bus;

        public BasePublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendAsync<T>(T template, CancellationToken token = default) where T : class
        {
            var endPoint = await _bus.GetPublishSendEndpoint<T>();
            await endPoint.Send(template, token);
        }
    }
}
