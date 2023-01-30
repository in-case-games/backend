using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using CaseApplication.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlServer(
#if DEBUG
        builder.Configuration["CaseApp:DevelopmentConnection"],
#else
        builder.Configuration["CaseApp:ProductionConnection"],
#endif
        b => b.MigrationsAssembly("CaseApplication.Api"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddTransient<IPromocodeUserByUserRepository, PromocodesUsedByUserRepository>();
builder.Services.AddTransient<IPromocodeTypeRepository, PromocodeTypeRepository>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}
