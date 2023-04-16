using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace InCase.IntegrationTests.Tests
{
    public class BaseApiTest
    {
        protected readonly ITestOutputHelper _output;
        protected string AccessToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjhjMTgzYjc2LTY4MGMtNDY1MC04ZTY2LWRlMTAyZDE3MGRkNCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im93bmVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkdJUyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Inl0X2ZlcmJyYXlAbWFpbC5ydSIsImV4cCI6MTY4MTkyOTc3OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdCJ9.WycgIJ3rEyC6Yn3uczRo6tUhulME-zJePLY9nX0XRycA0HN437Kui7bt5aJJJix_GhUKgOE_K-xwRxsQB1SHXw";
        protected string RefreshToken = "";
        private readonly IConfiguration _configuration;
        protected ApplicationDbContext Context { get; }

        public BaseApiTest(
            ITestOutputHelper output,
            IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention()
                .UseSqlServer(configuration["ConnectionStrings:DevelopmentConnection"])
                .Options;
            
            _configuration = configuration;

            Context = new ApplicationDbContext(options);

            _output = output;
        }
        private JwtSecurityToken GenerateToken(
            Claim[] claims,
            TimeSpan expiration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            return new(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"]!,
                claims,
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: credentials);
        }
        public string CreateToken(User user)
        {
            Claim[] claims = GenerateClaimsForAccessToken(user);
            TimeSpan expirationAccess = TimeSpan.FromMinutes(
                double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!));

            JwtSecurityToken token = GenerateToken(claims, expirationAccess);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Claim[] GenerateClaimsForAccessToken(User user)
        {
            //Find future data for claims
            string roleName = user.AdditionalInfo!.Role!.Name!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.Login!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
        }
        protected string GenerateString(int length = 8)
        {
            Random random = new Random();
        
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #region Начальные данные
        protected async Task InitializeUserDependency(Guid guid, string roleName = "user")
        {
            UserRole? userRole = await Context.UserRoles.FirstOrDefaultAsync(x => x.Name == roleName);

            UserAdditionalInfo userInfo = new()
            {
                IsConfirmed = true,
                Balance = 99999999M,
                Role = userRole!,
                RoleId = userRole!.Id,
                IsNotifyEmail = true,
                IsGuestMode = false
            };
            User user = new()
            {
                Id = guid,
                Login = $"{GenerateString()}UserApiTest",
                Email = $"{GenerateString()}@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1",
                AdditionalInfo = userInfo,
            };

            AccessToken = CreateToken(user);

            await Context.Users.AddAsync(user);
            await Context.UserAdditionalInfos.AddAsync(userInfo);
            await Context.SaveChangesAsync();
        }

        protected async Task RemoveUserDependency(Guid guid)
        {
            User? user = await Context.Users.FindAsync(guid);
            Context.Users.Remove(user!);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
