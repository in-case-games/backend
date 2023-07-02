using Identity.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.BLL.Services
{
    public class RestrictionManagerService : IHostedService
    {
        private readonly IUserRestrictionService _userRestrictionService;
        private readonly ILogger<RestrictionManagerService> _logger;

        public RestrictionManagerService(
            IServiceProvider serviceProvider,
            ILogger<RestrictionManagerService> logger)
        {
            _userRestrictionService = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<IUserRestrictionService>();
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
                    await _userRestrictionService.DoWorkManagerAsync(stoppingToken);
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
