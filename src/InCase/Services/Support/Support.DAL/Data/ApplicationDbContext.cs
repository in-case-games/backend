using Microsoft.EntityFrameworkCore;
using Support.DAL.Entities;
using System.Reflection;

namespace Support.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<SupportTopicAnswer> SupportTopicAnswers => Set<SupportTopicAnswer>();
    public DbSet<AnswerImage> AnswerImages => Set<AnswerImage>();
    public DbSet<SupportTopic> SupportTopics => Set<SupportTopic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}