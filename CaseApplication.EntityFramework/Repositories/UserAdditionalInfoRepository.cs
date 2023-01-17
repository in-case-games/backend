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
            await _context.UserAdditionalInfo.AddAsync(info);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInfo(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            UserAdditionalInfo? info = await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.Id == id);
            if (info != null)
            {
                _context.UserAdditionalInfo.Remove(info);
                await _context.SaveChangesAsync();
            }
            return (info != null);
        }

        public async Task<UserAdditionalInfo> GetInfo(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.UserAdditionalInfo.FirstOrDefaultAsync(x => x.Id == id)
                ?? new();
        }

        public async Task<bool> UpdateInfo(UserAdditionalInfo info)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            _context.Set<UserAdditionalInfo>().Update(info);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
