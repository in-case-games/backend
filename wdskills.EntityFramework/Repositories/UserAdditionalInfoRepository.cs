﻿using Microsoft.EntityFrameworkCore;
using wdskills.DomainLayer.Entities;
using wdskills.DomainLayer.Repositories;
using wdskills.EntityFramework.Data;

namespace wdskills.EntityFramework.Repositories
{
    public class UserAdditionalInfoRepository : IUserAdditionalInfoRepository
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        public UserAdditionalInfoRepository(ApplicationDbContextFactory contextFactory) {
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
