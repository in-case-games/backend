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
        public async Task<UserRestriction> GetRestriction(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await _context.UserRestriction.FirstOrDefaultAsync(x => x.Id == id);
            
            return searchRestriction 
                ?? throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");
        }
        public async Task<IEnumerable<UserRestriction>> GetAllRestrictions(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.UserRestriction.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> CreateRestriction(UserRestriction userRestriction)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            User? searchUser = await _context.User.FirstOrDefaultAsync(x => x.Id == userRestriction.UserId);
            
            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            await _context.UserRestriction.AddAsync(userRestriction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRestriction(UserRestriction userRestriction)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await _context.UserRestriction.FirstOrDefaultAsync(x => x.Id == userRestriction.Id);
            User? searchUser = await _context.User.FirstOrDefaultAsync(x => x.Id == userRestriction.UserId);

            if (searchRestriction is null) 
                throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");
            if(searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            _context.Entry(searchRestriction).CurrentValues.SetValues(searchUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRestriction(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRestriction? searchRestriction = await _context.UserRestriction.FirstOrDefaultAsync(x => x.Id == id);

            if (searchRestriction is null)
                throw new Exception("There is no such user restriction in the database, " +
                "review what data comes from the api");

            _context.UserRestriction.Remove(searchRestriction);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
