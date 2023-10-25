using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.Services
{
    public class WithdrawManagerService : IHostedService
    {
        private readonly IWithdrawService _withdrawService;

        public WithdrawManagerService(IServiceProvider serviceProvider)
        {
            _withdrawService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IWithdrawService>();
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
                    await _withdrawService.WithdrawStatusManagerAsync(10, stoppingToken);
                }
                catch (Exception)
                {
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
