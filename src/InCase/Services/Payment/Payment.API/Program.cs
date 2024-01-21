using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Payment.API.Middlewares;
using Payment.BLL.Interfaces;
using Payment.BLL.MassTransit;
using Payment.BLL.MassTransit.Consumers;
using Payment.BLL.Services;
using Payment.DAL.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

builder.Logging.AddConfiguration(configuration).ClearProviders().AddNLog();

builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => {
        options.UseSnakeCaseNamingConvention();
        options.UseNpgsql(
        #if DEBUG
        builder.Configuration["ConnectionStrings:DevelopmentConnection"],
        #else
        builder.Configuration["ConnectionStrings:ProductionConnection"],
        #endif
        b => b.MigrationsAssembly("Payment.API"));
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
builder.Services.AddSingleton<IEncryptorService, EncryptorService>();
builder.Services.AddSingleton<IResponseService, ResponseService>();
builder.Services.AddSingleton<IGameMoneyService, GameMoneyService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserPaymentsService, UserPaymentsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserPromocodeService, UserPromocodeService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();
    x.AddConsumer<UserPromocodeConsumer>();
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["MassTransit:Uri"]!), h =>
        {
            h.Username(builder.Configuration["MassTransit:Username"]!);
            h.Password(builder.Configuration["MassTransit:Password"]!);
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
            e.ConfigureConsumer<UserPromocodeConsumer>(context);
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
