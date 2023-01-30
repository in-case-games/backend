using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationDbContext> context)
        {
            _contextFactory = context;
        }

        public async Task<User> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.Id == id) ?? 
                throw new("There is no such user in the database, " +
                "review what data comes from the api");
        }

        public async Task<User> GetByEmail(string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.UserEmail == email) ?? 
                throw new("There is no such user in the database, " +
                "review what data comes from the api");
        }

        public async Task<User> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.UserLogin == login) ?? 
                throw new("There is no such user in the database, " + 
                "review what data comes from the api");
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.ToListAsync();
        }

        public async Task<bool> Create(User user)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(User user)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.Id == user.Id);

            if(searchUser is null) throw new Exception("You cannot change the guid, " +
                "review what data comes from the api");

            context.Entry(searchUser).CurrentValues.SetValues(user);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.Id == id);

            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            context.User.Remove(searchUser);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
