using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var policyName = "CorsPolicy";

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder => {
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();

await app.UseOcelot();
app.UseCors(policyName);
app.UseAuthorization();

app.Run();
