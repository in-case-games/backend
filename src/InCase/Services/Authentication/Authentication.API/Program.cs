using Authentication.API.Middlewares;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.BLL.MassTransit.Consumers;
using Authentication.BLL.Services;
using Authentication.DAL.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{env}.json")
    .Build();

builder.Configuration.AddEnvironmentVariables();
builder.Logging.AddConfiguration(configuration).ClearProviders().AddNLog();
builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => {
        options.UseSnakeCaseNamingConvention();
        options.UseNpgsql(builder.Configuration[$"ConnectionStrings:{env}"], b => b.MigrationsAssembly("Authentication.API"));
    }
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration[$"JWT:ValidIssuer:{env}"]!,

            ValidateAudience = true,
            ValidAudience = builder.Configuration[$"JWT:ValidAudience:{env}"]!,
            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration[$"JWT:Secret:{env}"]!)),

            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Example: \"Bearer [token]\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSingleton<BasePublisher>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationConfirmService, AuthenticationConfirmService>();
builder.Services.AddScoped<IAuthenticationSendingService, AuthenticationSendingService>();
builder.Services.AddScoped<IUserRestrictionService, UserRestrictionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAdditionalInfoService, UserAdditionalInfoService>();
builder.Services.AddHostedService<UserManagerService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRestrictionConsumer>();
    x.AddConsumer<UserAdditionalInfoConsumer>();
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration[$"MassTransit:Uri:{env}"]!), h =>
        {
            h.Username(builder.Configuration[$"MassTransit:Username:{env}"]!);
            h.Password(builder.Configuration[$"MassTransit:Password:{env}"]!);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<UserRestrictionConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<UserAdditionalInfoConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();