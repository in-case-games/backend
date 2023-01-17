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

        public async Task<bool> CreateInfo(UserAdditionalInfo info)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRole? standartRole = await _context.UserRole.FirstOrDefaultAsync(x => x.RoleName == "user");
            if (standartRole is null) throw new Exception();

            info.UserRoleId = standartRole.Id;
            await _context.UserAdditionalInfo.AddAsync(info);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteInfo(Guid infoId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserAdditionalInfo? info = await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.Id == infoId);
            
            if (info is not null)
            {
                _context.UserAdditionalInfo.Remove(info);
                await _context.SaveChangesAsync();
            }

            return (info is not null);
        }

        public async Task<UserAdditionalInfo> GetInfo(Guid userId)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.UserId == userId)
                ?? new();
        }

        public async Task<bool> UpdateInfo(UserAdditionalInfo info)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            _context.Set<UserAdditionalInfo>().Update(info);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserRole> GetRoleToName(string nameRole)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.UserRole.FirstOrDefaultAsync(x => x.RoleName == nameRole) 
                ?? new();
        }

        public async Task<UserRole> GetRoleToId(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            return await _context.UserRole.FirstOrDefaultAsync(x => x.Id == id)
                ?? new();
        }
    }
}
