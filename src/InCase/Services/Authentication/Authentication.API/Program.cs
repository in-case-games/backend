using Authentication.API.Middlewares;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.BLL.MassTransit.Consumers;
using Authentication.BLL.Services;
using Authentication.DAL.Data;
using Identity.BLL.MassTransit.Consumers;
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
        b => b.MigrationsAssembly("Authentication.API"));
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
            ep.ConfigureConsumer<UserRestrictionConsumer>(provider);
        });
        cfg.ReceiveEndpoint(ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(4, 100));
            ep.ConfigureConsumer<UserAdditionalInfoConsumer>(provider);
        });
    }));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

var app = builder.Build();

using (var Scope = app.Services.CreateScope())
{
    var context = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

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

public partial class Program { }