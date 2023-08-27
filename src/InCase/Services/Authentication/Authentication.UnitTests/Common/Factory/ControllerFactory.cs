using Authentication.BLL.MassTransit.Consumers;
using Authentication.BLL.MassTransit;
using Authentication.BLL.Services;
using Identity.BLL.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Authentication.DAL.Data;

namespace Authentication.UnitTests.Common.Factory;

public abstract class ControllerFactory
{
	private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();
	protected ServiceProvider provider; 
	protected JwtService jwtService;
	protected BasePublisher basePublisher;
	public ControllerFactory()
	{
		provider = new ServiceCollection()
            .AddSingleton<BasePublisher>()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UserRestrictionConsumer>();
                cfg.AddConsumer<UserAdditionalInfoConsumer>();
                cfg.SetKebabCaseEndpointNameFormatter();
            }).BuildServiceProvider(true);

		basePublisher = provider.GetRequiredService<BasePublisher>();
		jwtService = new JwtService(_configuration);
	}
	abstract public ControllerBase Create(ApplicationDbContext context);
}
