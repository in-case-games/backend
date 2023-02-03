using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using CaseApplication.EntityFramework.Repositories;
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
        b => b.MigrationsAssembly("CaseApplication.Api"))
);
// TODO: Configure CORS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

builder.Services.AddAuthorization();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserAdditionalInfoRepository, UserAdditionalInfoRepository>();
builder.Services.AddTransient<IGameItemRepository, GameItemRepository>();
builder.Services.AddTransient<IGameCaseRepository, GameCaseRepository>();
builder.Services.AddTransient<ICaseInventoryRepository, CaseInventoryRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IUserRestrictionRepository, UserRestrictionRepository>();
builder.Services.AddTransient<IUserInventoryRepository, UserInventoryRepository>();
builder.Services.AddTransient<IUserHistoryOpeningCasesRepository, UserHistoryOpeningCasesRepository>();
builder.Services.AddTransient<INewsRepository, NewsRepository>();
builder.Services.AddTransient<ISiteStatisticsRepository, SiteStatisticsRepository>();
builder.Services.AddTransient<IPromocodeRepository, PromocodeRepository>();
builder.Services.AddTransient<IPromocodeUsedByUserRepository, PromocodesUsedByUserRepository>();
builder.Services.AddTransient<IPromocodeTypeRepository, PromocodeTypeRepository>();
builder.Services.AddTransient<IUserTokensRepository, UserTokensRepository>();

builder.Services.AddSingleton<EncryptorHelper>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
