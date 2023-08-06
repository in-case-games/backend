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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => {
        options.UseSnakeCaseNamingConvention();
        options.UseNpgsql(
#if DEBUG
        builder.Configuration["ConnectionStrings:DevelopmentConnection"],
#else
        builder.Configuration["ConnectionStrings:ProductionConnection"],
#endif
        b => b.MigrationsAssembly("Game.API"));
    }
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"]!,

            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"]!,
            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),

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
        Description = "Example: \"Bearer 1safsfsdfdfd\"",
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
builder.Services.AddScoped<IUserPromocodeService, UserPromocodeService>();
builder.Services.AddScoped<IGameItemService, GameItemService>();
builder.Services.AddScoped<ILootBoxService, LootBoxService>();
builder.Services.AddScoped<ILootBoxInventoryService, LootBoxInventoryService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();
    x.AddConsumer<UserPromocodeConsumer>();
    x.AddConsumer<UserPaymentConsumer>();
    x.AddConsumer<GameItemConsumer>();
    x.AddConsumer<LootBoxBannerConsumer>();
    x.AddConsumer<LootBoxConsumer>();
    x.AddConsumer<LootBoxInventoryConsumer>();
    x.AddConsumer<UserInventoryBackConsumer>();
    x.SetKebabCaseEndpointNameFormatter();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(builder.Configuration["MassTransit:Uri"]!), h =>
        {
            h.Username(builder.Configuration["MassTransit:Username"]!);
            h.Password(builder.Configuration["MassTransit:Password"]!);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<UserConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<UserPromocodeConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<UserPaymentConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<GameItemConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<LootBoxBannerConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<LootBoxConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<LootBoxInventoryConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<UserInventoryBackConsumer>(provider);
        });
    }));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CancellationTokenHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();