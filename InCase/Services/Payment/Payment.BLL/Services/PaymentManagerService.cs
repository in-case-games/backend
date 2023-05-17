using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Payment.BLL.Interfaces;

namespace Payment.BLL.Services
{
    public class PaymentManagerService : IHostedService
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentManagerService> _logger;

        public PaymentManagerService(
            IPaymentService paymentService,
            ILogger<PaymentManagerService> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = DoWork(cancellationToken);

            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Start work manager");
                    await _paymentService.DoWorkManagerAsync(stoppingToken);
                }
                catch (Exception)
                {
                    _logger.LogInformation("BB work manager");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
