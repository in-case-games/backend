using MassTransit;
using Microsoft.Extensions.Configuration;

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
            if (template is not null)
            {
                var endPoint = await _bus.GetPublishSendEndpoint<T>();
                await endPoint.Send(template, token);
            }
        }
    }
}
