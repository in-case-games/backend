using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public NewsRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<News> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            News? searchNews = await context.News.FirstOrDefaultAsync(x => x.Id == id);

            return searchNews ?? throw new("There is no such news in the database, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.News.ToListAsync();
        }

        public async Task<bool> Create(News news)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            await context.News.AddAsync(news);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(News news)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            
            News? searchNews = await context.News.FirstOrDefaultAsync(x => x.Id == news.Id);

            if(searchNews is null) throw new("There is no such news in the database, " +
                "review what data comes from the api");

            context.Entry(searchNews).CurrentValues.SetValues(news);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            News? searchNews = await context.News.FirstOrDefaultAsync(x => x.Id == id);

            if (searchNews is null) throw new("There is no such news in the database, " +
                "review what data comes from the api");

            context.Remove(searchNews);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
