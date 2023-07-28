using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Withdraw.BLL.MassTransit
{
    public class BasePublisher
    {
        private readonly IConfiguration _cfg;
        private readonly IBus _bus;

        public BasePublisher(IConfiguration cfg, IBus bus)
        {
            _cfg = cfg;
            _bus = bus;
        }

        public async Task SendAsync<T>(T template, string name, CancellationToken cancellationToken = default)
        {
            if (template is not null)
            {
                Uri uri = new(_cfg["MassTransit:Uri"] + name);
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(template, cancellationToken);
            }
        }
    }
}
