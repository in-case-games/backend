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

        public async Task<UserAdditionalInfo> GetInfo(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? searchInfo = await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.UserId == userId);

            return searchInfo ?? throw new Exception("There is no such information in the database, " +
                "review what data comes from the api");
        }

        public async Task<bool> CreateInfo(UserAdditionalInfo info)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            User? searchUser = await _context.User.FirstOrDefaultAsync(x => x.Id == info.UserId);
            UserRole? searchRole = await _context.UserRole.FirstOrDefaultAsync(x => x.RoleName == "user");

            if (searchRole is null) throw new Exception("Add standard roles to the database");
            if (searchUser is null) throw new Exception("There is no user with this Guid in the database, " +
                "review what data comes from the api");

            info.Id = Guid.NewGuid();
            info.UserRoleId = searchRole!.Id;

            await _context.UserAdditionalInfo.AddAsync(info);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateInfo(UserAdditionalInfo info)
        {
            UserAdditionalInfo searchInfo = await GetInfo(info.Id);

            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            _context.Entry(searchInfo).CurrentValues.SetValues(info);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteInfo(Guid infoId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? searchInfo = await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.Id == infoId);

            if(searchInfo is null) throw new Exception("There is no such information in the database, " +
                "review what data comes from the api");

            _context.UserAdditionalInfo.Remove(searchInfo);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
