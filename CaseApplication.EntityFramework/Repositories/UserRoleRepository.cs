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
        public async Task<UserRole?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserRole?> GetByName(string name)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.RoleName == name);
        }

        public async Task<List<UserRole>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.UserRole
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> Create(UserRole role)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            role.Id = new Guid();

            await context.UserRole.AddAsync(role);
            await context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> Update(UserRole role)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? searchUserRole = await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == role.Id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            context.Entry(searchUserRole).CurrentValues.SetValues(role);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? searchUserRole = await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            context.UserRole.Remove(searchUserRole);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
