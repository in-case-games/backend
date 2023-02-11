using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using AutoMapper;
using CaseApplication.DomainLayer.Dtos;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserAdditionalInfoRepository : IUserAdditionalInfoRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserAdditionalInfoRepository(IDbContextFactory<ApplicationDbContext> contextFactory) {
            _contextFactory = contextFactory;
        }
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfoDto, UserAdditionalInfo>();
        });

        public async Task<UserAdditionalInfo?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.Id == id);

            return info;
        }

        public async Task<UserAdditionalInfo?> GetByUserId(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context.UserAdditionalInfo
                .AsNoTracking()
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return info;
        }

        public async Task<bool> Create(UserAdditionalInfoDto infoDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserRole? searchRole = await context.UserRole
                .AsNoTracking().FirstOrDefaultAsync(x => x.RoleName == "user");

            if (searchRole is null) throw new Exception("Add standard roles to the database");

            IMapper? mapper = _mapperConfiguration.CreateMapper();
            UserAdditionalInfo info = mapper.Map<UserAdditionalInfo>(infoDto);
            info.Id = Guid.NewGuid();
            info.UserRoleId = searchRole!.Id;

            await context.UserAdditionalInfo.AddAsync(info);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserAdditionalInfo info)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? searchInfo = await context.UserAdditionalInfo
                .AsNoTracking()
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
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? searchInfo = await context.UserAdditionalInfo
                .AsNoTracking()
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
