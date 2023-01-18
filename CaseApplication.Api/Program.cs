using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using CaseApplication.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration["CaseApp:DevelopmentConnection"],
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
