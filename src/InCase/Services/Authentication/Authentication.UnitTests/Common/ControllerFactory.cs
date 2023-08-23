using Authentication.API.Controllers;
using Authentication.BLL.MassTransit;
using Authentication.BLL.MassTransit.Consumers;
using Authentication.BLL.Services;
using Authentication.DAL.Data;
using Identity.BLL.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.UnitTests.Common
{
    public class ControllerFactory
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();
        public static AuthenticationController CreateAuthenticationController(
            ApplicationDbContext context)
        {
            var provider = new ServiceCollection()
            .AddSingleton<BasePublisher>()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UserRestrictionConsumer>();
                cfg.AddConsumer<UserAdditionalInfoConsumer>();
                cfg.SetKebabCaseEndpointNameFormatter();
            }).BuildServiceProvider(true);

            var basePublisher = provider.GetRequiredService<BasePublisher>();

            JwtService jwtService = new JwtService(_configuration);

            AuthenticationService service = new AuthenticationService(context,
                jwtService,
                basePublisher);
            AuthenticationController controller =
                new AuthenticationController(service);

            return controller;
        }
        public static AuthenticationConfirmController CreateAuthenticationConfirmController(
            ApplicationDbContext context)
        {
            var provider = new ServiceCollection()
            .AddSingleton<BasePublisher>()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UserRestrictionConsumer>();
                cfg.AddConsumer<UserAdditionalInfoConsumer>();
                cfg.SetKebabCaseEndpointNameFormatter();
            }).BuildServiceProvider(true);

            var basePublisher = provider.GetRequiredService<BasePublisher>();

            JwtService jwtService = new JwtService(_configuration);

            AuthenticationService authService = new AuthenticationService(context,
                jwtService,
                basePublisher);

            AuthenticationConfirmService service = new AuthenticationConfirmService(context,
                authService,
                jwtService,
                basePublisher);
            AuthenticationConfirmController controller =
                new AuthenticationConfirmController(service);

            return controller;
        }
    }
}
