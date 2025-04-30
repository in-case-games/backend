using EmailSender.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.API.Initializers;
public sealed class Initializer(IServiceProvider serviceProvider)
{
    private static bool IsEnd = false; 

    private readonly object _locker = new();
	private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void InitializeDb() {
        if (IsEnd) return;

        lock (_locker) {
            while (!IsEnd) {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();
                Seed(context);
            }
        }
    }

	private static void Seed(ApplicationDbContext context) {
        IsEnd = true;
	}
}