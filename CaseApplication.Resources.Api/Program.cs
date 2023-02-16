using CaseApplication.Infrastructure.Services;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlServer(
#if DEBUG
        builder.Configuration["ConnectionStrings:DevelopmentConnection"],
#else
        builder.Configuration["ConnectionStrings:ProductionConnection"],
#endif
        b => b.MigrationsAssembly("CaseApplication.Resources.Api"))
);
// TODO: Configure CORS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuers = new List<string>()
            {
                builder.Configuration["JWT:ValidIssuer"]!,
                builder.Configuration["JWT:ValidIssuers:1"]!,
                builder.Configuration["JWT:ValidIssuers:2"]!,
            },

            ValidateAudience = true,
            ValidAudiences = new List<string>()
            {
                builder.Configuration["JWT:ValidAudience"]!,
                builder.Configuration["JWT:ValidAudiences:1"]!,
                builder.Configuration["JWT:ValidAudiences:2"]!,
            },
            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),

            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<EmailHelper>();

builder.Services.AddSingleton<EncryptorHelper>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<ValidationService>();

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
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

public partial class Program {}
