using InCase.Infrastructure.Data;
using InCase.Infrastructure.Middleware;
using InCase.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string _policyName = "CorsPolicy";

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSnakeCaseNamingConvention();

    options.UseSqlServer(
#if DEBUG
    builder.Configuration["ConnectionStrings:DevelopmentConnection"],
#else
        builder.Configuration["ConnectionStrings:ProductionConnection"],
#endif
    b => b.MigrationsAssembly("Resources.Api"));
});

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
        Description = "JWT Uebanization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your dick is fall in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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

builder.Services.AddSingleton<ResponseService>();
builder.Services.AddSingleton<TradeMarketService>();
builder.Services.AddSingleton<WithdrawItemService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _policyName,
        builder => {
            builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
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

app.UseMiddleware<TaskCancellationTokenHandleMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(_policyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class HostResourcesApiTests { }