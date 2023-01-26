using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserHistoryOpeningCasesRepository : IUserHistoryOpeningCasesRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserHistoryOpeningCasesRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserHistoryOpeningCases> Get(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserHistoryOpeningCases? searchUserHistory = await _context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id);

            return searchUserHistory ?? throw new("There is no such user history in the database, " +
                "review what data comes from the api");
        }
        public async Task<IEnumerable<UserHistoryOpeningCases>> GetAll(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            IEnumerable<UserHistoryOpeningCases> userHistoryOpeningCases = await _context
                .UserHistoryOpeningCases
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return userHistoryOpeningCases;
        }

        public async Task<bool> Create(UserHistoryOpeningCases userHistory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            GameCase? searchGameCase = await _context
                .GameCase
                .FirstOrDefaultAsync(x => x.Id == userHistory.GameCaseId);

            GameItem? searchGameItem = await _context
                .GameItem
                .FirstOrDefaultAsync(x => x.Id == userHistory.GameItemId);

            User? searchUser = await _context
                .User
                .FirstOrDefaultAsync(x => x.Id == userHistory.UserId);

            if (searchGameCase is null) throw new Exception("There is no such case, " +
                "review what data comes from the api");
            if (searchGameItem is null) throw new Exception("There is no such item, " +
                "review what data comes from the api");
            if (searchUser is null) throw new Exception("There is no such user, " +
                "review what data comes from the api");

            await _context.UserHistoryOpeningCases.AddAsync(userHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserHistoryOpeningCases userHistory)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserHistoryOpeningCases? searchUserHistory = await _context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == userHistory.Id);

            if (searchUserHistory is null) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            _context.Entry(searchUserHistory).CurrentValues.SetValues(userHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserHistoryOpeningCases? searchUserHistory = await _context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserHistory is null) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            _context.UserHistoryOpeningCases.Remove(searchUserHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAll(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            List<UserHistoryOpeningCases>? searchUserHistories = await _context
                .UserHistoryOpeningCases
                .Where(x => x.UserId == userId).ToListAsync();

            if(searchUserHistories.Count == 0) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            _context.UserHistoryOpeningCases.RemoveRange(searchUserHistories);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
