using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserRestrictionRepository : IUserRestrictionRepository
    {
        public readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserRestrictionRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserRestriction> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await context
                .UserRestriction
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return searchRestriction 
                ?? throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");
        }
        public async Task<IEnumerable<UserRestriction>> GetAll(Guid userId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.UserRestriction.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> Create(UserRestriction userRestriction)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            User? searchUser = await context
                .User
                .FirstOrDefaultAsync(x => x.Id == userRestriction.UserId);
            
            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            await context.UserRestriction.AddAsync(userRestriction);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserRestriction userRestriction)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await context
                .UserRestriction
                .FirstOrDefaultAsync(x => x.Id == userRestriction.Id);
            User? searchUser = await context
                .User
                .FirstOrDefaultAsync(x => x.Id == userRestriction.UserId);

            if (searchRestriction is null) 
                throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");
            if(searchUser is null)
                throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            context.Entry(searchRestriction).CurrentValues.SetValues(userRestriction);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await context
                .UserRestriction
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchRestriction is null)
                throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");

            context.UserRestriction.Remove(searchRestriction);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
