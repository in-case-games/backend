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
        public async Task<bool> IsUniqueSalt(string salt)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.PasswordSalt == salt);

            return searchUser is null;
        }
        public async Task<User?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.UserEmail == email);
        }

        public async Task<User?> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.UserLogin == login);
        }

        public async Task<User?> GetByParameters(User user)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User
                .FirstOrDefaultAsync(x => 
                x.UserEmail == user.UserEmail || 
                x.Id == user.Id ||
                x.UserLogin == user.UserLogin);
        }

        public async Task<bool> Create(User user)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(User oldUser, User newUser)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            context.Entry(oldUser).CurrentValues.SetValues(newUser);
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
