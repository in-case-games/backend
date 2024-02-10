using Microsoft.EntityFrameworkCore;
using Review.DAL.Entities;
using System.Reflection;

namespace Review.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ReviewImage> ReviewImages  => Set<ReviewImage>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserReview> UserReviews => Set<UserReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}