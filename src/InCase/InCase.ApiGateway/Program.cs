using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var policyName = "CorsPolicy";

var configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

builder.Logging.AddConfiguration(configuration).ClearProviders().AddNLog();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder => {
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseCors(policyName);
await app.UseOcelot();
app.UseAuthorization();

app.Run();
