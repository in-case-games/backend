using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserTokensRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserToken?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserToken.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<UserToken?> GetByToken(Guid userId, string token)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserToken
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RefreshToken == token);
        }

        public async Task<UserToken?> GetByIp(Guid userId, string ip)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserToken
                .FirstOrDefaultAsync(x => x.UserId == userId && x.UserIpAddress == ip);
        }

        public async Task<List<UserToken>> GetAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserToken.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<bool> Create(UserToken token)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            token.Id = Guid.NewGuid();

            await context.UserToken.AddAsync(token);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Update(UserToken token)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserToken? userToken = await context.UserToken.FirstOrDefaultAsync(x => x.Id == token.Id);

            if (userToken == null)
            {
                throw new Exception("There is no such token, " +
                    "review what data comes from the api");
            }

            context.Entry(token).CurrentValues.SetValues(token);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserToken? userToken = await context.UserToken.FirstOrDefaultAsync(x => x.Id == id);

            if (userToken == null)
            {
                throw new Exception("There is no such token, " +
                    "review what data comes from the api");
            }

            context.UserToken.Remove(userToken);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByToken(Guid userId, string refreshToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserToken? userToken = await context.UserToken
                .FirstOrDefaultAsync(x => x.Id == userId && x.RefreshToken == refreshToken);

            if (userToken == null)
            {
                throw new Exception("There is no such token, " +
                    "review what data comes from the api");
            }

            context.UserToken.Remove(userToken);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserToken> userTokens = await context.UserToken
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if(userTokens.Count == 0)
            {
                throw new Exception("There is no such token, " +
                    "review what data comes from the api");
            }

            context.UserToken.RemoveRange(userTokens);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
