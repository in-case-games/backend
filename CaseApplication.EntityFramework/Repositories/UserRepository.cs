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
            _contextFactory= context;
        }
        public async Task<bool> CreateUser(User user)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            User? user = await _context.User.FirstOrDefaultAsync(x => x.Id == id);
            if(user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
            return (user != null);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUser(string email)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.User.FirstOrDefaultAsync(x => x.UserEmail == email) ?? new();
        }

        public async Task<bool> UpdateUser(User user)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
