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
        public async Task<UserRole> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.UserRole.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new("There is no such role in the database, " +
                "review what data comes from the api");
        }

        public async Task<UserRole> GetByRole(UserRole role)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRole? searchRole = await context.UserRole.FirstOrDefaultAsync(x => x.Id == role.Id);

            searchRole ??= await context.UserRole.FirstOrDefaultAsync(x => x.RoleName == role.RoleName);

            return searchRole ?? throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            List<UserRole> searchRoles = await context.UserRole.ToListAsync();

            return searchRoles;
        }

        public async Task<bool> Create(UserRole role)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRole? searchRole = await context.UserRole.FirstOrDefaultAsync(x => x.RoleName == role.RoleName);
            if(searchRole is not null) throw new Exception("There is such role in the database, " +
                "review what data comes from the api");

            role.Id = new Guid();

            await context.UserRole.AddAsync(role);
            await context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> Update(UserRole role)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRole? searchUserRole = await context.UserRole.FirstOrDefaultAsync(x => x.Id == role.Id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            context.Entry(searchUserRole).CurrentValues.SetValues(role);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            UserRole? searchUserRole = await context.UserRole.FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserRole is null) throw new Exception("There is no such role in the database, " +
                "review what data comes from the api");

            context.UserRole.Remove(searchUserRole);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
