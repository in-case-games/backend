using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

const string policyName = "CorsPolicy";

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile($"ocelot.{env}.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

builder.Configuration.AddEnvironmentVariables();

builder.Logging.AddConfiguration(configuration).ClearProviders().AddNLog();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        cfg => {
            cfg.WithOrigins(env == "Production" ? "https://in-case.games" : "http://localhost:3000")
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
