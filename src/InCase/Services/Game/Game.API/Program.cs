using Game.API.Middlewares;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.BLL.MassTransit.Consumers;
using Game.BLL.Services;
using Game.DAL.Data;
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
        options.UseNpgsql(builder.Configuration[$"ConnectionStrings:{env}"], b => b.MigrationsAssembly("Game.API"));
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

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[$"JWT:Secret:{env}"]!)),

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
builder.Services.AddScoped<IUserPathBannerService, UserPathBannerService>();
builder.Services.AddScoped<IUserAdditionalInfoService, UserAdditionalInfoService>();
builder.Services.AddScoped<IUserOpeningService, UserOpeningService>();
builder.Services.AddScoped<ILootBoxOpeningService, LootBoxOpeningService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserPromoCodeService, UserPromoCodeService>();
builder.Services.AddScoped<IGameItemService, GameItemService>();
builder.Services.AddScoped<ILootBoxService, LootBoxService>();
builder.Services.AddScoped<ILootBoxInventoryService, LootBoxInventoryService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();
    x.AddConsumer<UserPromoCodeConsumer>();
    x.AddConsumer<UserPaymentConsumer>();
    x.AddConsumer<GameItemConsumer>();
    x.AddConsumer<LootBoxBannerConsumer>();
    x.AddConsumer<LootBoxConsumer>();
    x.AddConsumer<LootBoxInventoryConsumer>();
    x.AddConsumer<UserInventoryBackConsumer>();
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
            e.ConfigureConsumer<UserConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<UserPromoCodeConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<UserPaymentConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<GameItemConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<LootBoxBannerConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<LootBoxConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<LootBoxInventoryConsumer>(context);
        });
        cfg.ReceiveEndpoint(e =>
        {
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(4, 100));
            e.ConfigureConsumer<UserInventoryBackConsumer>(context);
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