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

        public async Task<UserHistoryOpeningCases?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<UserHistoryOpeningCases>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserHistoryOpeningCases.ToListAsync();
        }

        public async Task<List<UserHistoryOpeningCases>> GetAllById(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserHistoryOpeningCases
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> Create(UserHistoryOpeningCases userHistory)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            await context.UserHistoryOpeningCases.AddAsync(userHistory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserHistoryOpeningCases userHistory)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? searchUserHistory = await context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == userHistory.Id);

            if (searchUserHistory is null) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            context.Entry(searchUserHistory).CurrentValues.SetValues(userHistory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserHistoryOpeningCases? searchUserHistory = await context
                .UserHistoryOpeningCases
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserHistory is null) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            context.UserHistoryOpeningCases.Remove(searchUserHistory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserHistoryOpeningCases>? userHistories = await context.UserHistoryOpeningCases
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if(userHistories.Count == 0) throw new Exception("There is no such user history, " +
                "review what data comes from the api");

            context.UserHistoryOpeningCases.RemoveRange(userHistories);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
