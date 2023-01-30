using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserAdditionalInfoRepository : IUserAdditionalInfoRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserAdditionalInfoRepository(IDbContextFactory<ApplicationDbContext> contextFactory) {
            _contextFactory = contextFactory;
        }

        public async Task<UserAdditionalInfo> Get(Guid userId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? searchInfo = await context
                .UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return searchInfo ?? throw new Exception("There is no such information in the database, " +
                "review what data comes from the api");
        }

        public async Task<bool> Create(UserAdditionalInfo info)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.Id == info.UserId);
            UserRole? searchRole = await context.UserRole.FirstOrDefaultAsync(x => x.RoleName == "user");

            if (searchRole is null) throw new Exception("Add standard roles to the database");
            if (searchUser is null) {
                throw new Exception("There is no user with this Guid in the database, " +
                    "review what data comes from the api");
            }

            info.Id = Guid.NewGuid();
            info.UserRoleId = searchRole!.Id;

            await context.UserAdditionalInfo.AddAsync(info);
            await context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Update(UserAdditionalInfo info)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? searchInfo = await context
                .UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == info.UserId);

            if(searchInfo is null) {
                throw new Exception("There is no such information in the database, " + 
                    "review what data comes from the api");
            }

            context.Entry(searchInfo).CurrentValues.SetValues(info);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? searchInfo = await context
                .UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.Id == id);

            if(searchInfo is null) {
                throw new Exception("There is no such information in the database, " + 
                    "review what data comes from the api");
            }

            context.UserAdditionalInfo.Remove(searchInfo);
            await context.SaveChangesAsync();
            
            return true;
        }
    }
}
