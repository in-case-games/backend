using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using CaseApplication.DomainLayer.Dtos;
using AutoMapper;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private MapperConfiguration mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.CreateMap<UserDto, User>();
        }
        );

        public UserRepository(IDbContextFactory<ApplicationDbContext> context)
        {
            _contextFactory = context;
        }
        public async Task<bool> IsUniqueSalt(string salt)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.PasswordSalt == salt);

            return searchUser is null;
        }
        public async Task<User?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User.FindAsync(id);

            if (user is null)
                return null;

            user = await MapUser(user);

            return user;
        }

        public async Task<List<User>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            List<User> users = await context.User.ToListAsync();

            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context.User.FirstOrDefaultAsync(x => x.UserEmail == email);
        }

        public async Task<User?> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context.User.FirstOrDefaultAsync(x => x.UserLogin == login);
            if (user is null)
                return null;

            user = await MapUser(user);

            return user;
        }

        public async Task<User?> GetByParameters(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context.User
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.Id == userDto.Id ||
                x.UserLogin == userDto.UserLogin);

            if (user is null)
                return null;

            user = await MapUser(user);

            return user;
        }

        public async Task<bool> Create(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            IMapper? mapper = mapperConfiguration.CreateMapper();
            User user = mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserDto oldUserDto, UserDto newUserDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            IMapper? mapper = mapperConfiguration.CreateMapper();
            User oldUser = mapper.Map<User>(oldUserDto);
            User newUser = mapper.Map<User>(newUserDto);

            context.Entry(oldUser).CurrentValues.SetValues(newUser);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUser = await context.User.FirstOrDefaultAsync(x => x.Id == id);

            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            context.User.Remove(searchUser);
            await context.SaveChangesAsync();

            return true;
        }
        private async Task<User> MapUser(User user)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserAdditionalInfo? info = await context
                .UserAdditionalInfo
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            List<UserRestriction> restriction = await context
                .UserRestriction
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
            List<UserInventory> inventories = await context
                .UserInventory
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
            List<UserHistoryOpeningCases> history = await context
                .UserHistoryOpeningCases
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
            List<PromocodesUsedByUser> promocodes = await context
                .PromocodeUsedByUsers
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            if (info is not null)
            {
                UserRole? userRole = await context.UserRole.FindAsync(info!.UserRoleId);
                info.UserRole = userRole;
            }
            
            user.UserAdditionalInfo = info;
            user.UserRestrictions = restriction;
            user.UserInventories = inventories;
            user.UserHistoryOpeningCases = history;
            user.PromocodesUsedByUsers = promocodes;

            return user;
        }
    }
}
