using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserRoleRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserRole> GetRole(UserRole role)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRole? searchRole = await _context.UserRole.FirstOrDefaultAsync(x => x.Id == role.Id);

            searchRole ??= await _context.UserRole.FirstOrDefaultAsync(x => x.RoleName == role.RoleName);

            return searchRole ?? throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<UserRole>> GetAllRoles()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            List<UserRole> searchRoles = await _context.UserRole.ToListAsync();

            return searchRoles;
        }

        public async Task<bool> CreateRole(UserRole role)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRole? searchRole = await _context.UserRole.FirstOrDefaultAsync(x => x.RoleName == role.RoleName);
            if(searchRole is not null) throw new Exception("There is such role in the database, " +
                "review what data comes from the api");

            role.Id = new Guid();

            await _context.UserRole.AddAsync(role);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> UpdateRole(UserRole role)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRole? searchUserRole = await _context.UserRole.FirstOrDefaultAsync(x => x.Id == role.Id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            _context.Entry(searchUserRole).CurrentValues.SetValues(role);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRole(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();

            UserRole? searchUserRole = await _context.UserRole.FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            _context.UserRole.Remove(searchUserRole);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
