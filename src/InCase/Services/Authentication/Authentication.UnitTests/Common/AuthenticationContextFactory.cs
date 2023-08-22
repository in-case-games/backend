using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.UnitTests.Common
{
    public class AuthenticationContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSnakeCaseNamingConvention()
                .UseInMemoryDatabase("in-memory")
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(options);
            context.Roles.AddRange(new UserRole[]
            {
                new()
                {
                    Name = "user"
                },
                new()
                {
                    Name = "admin"
                },
                new()
                {
                    Name = "owner"
                },
                new()
                {
                    Name = "bot"
                },
            });
            context.SaveChanges();

            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
