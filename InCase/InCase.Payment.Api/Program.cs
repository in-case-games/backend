using InCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{ 
    options.UseSnakeCaseNamingConvention();

    options.UseSqlServer(
    #if DEBUG
    builder.Configuration["ConnectionStrings:DevelopmentConnection"],
    #else
    builder.Configuration["ConnectionStrings:ProductionConnection"],
    #endif
    b => b.MigrationsAssembly("InCase.Resources.Api"));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
