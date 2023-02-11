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
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
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

            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<List<User>> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            List<User> users = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserEmail == email);
        }

        public async Task<User?> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserLogin == login);

            return user;
        }

        public async Task<User?> GetByParameters(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            User? user = await context
                .User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                x.UserEmail == userDto.UserEmail ||
                x.Id == userDto.Id ||
                x.UserLogin == userDto.UserLogin);

            return user;
        }

        public async Task<bool> Create(UserDto userDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IMapper? mapper = _mapperConfiguration.CreateMapper();

            User user = mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserDto oldUserDto, UserDto newUserDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            IMapper? mapper = _mapperConfiguration.CreateMapper();
            User oldUser = mapper.Map<User>(oldUserDto);
            User newUser = mapper.Map<User>(newUserDto);

            context.Entry(oldUser).CurrentValues.SetValues(newUser);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUser = await context.User
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (searchUser is null) throw new Exception("There is no such user in the database, " +
                "review what data comes from the api");

            context.User.Remove(searchUser);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
