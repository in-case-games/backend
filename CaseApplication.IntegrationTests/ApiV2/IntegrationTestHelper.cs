using AutoMapper;
using Azure.Core;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CaseApplication.IntegrationTests.ApiV2
{
    public class IntegrationTestHelper
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .Build();
        private readonly JwtHelper _jwtHelper;
        protected readonly MapperConfiguration MapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfo, UserAdditionalInfoDto>();
        });
        protected string AccessToken { get; set; } = string.Empty;
        protected User User { get; set; } = new();
        protected ApplicationDbContext Context { get; }

        public IntegrationTestHelper()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer(_configuration["ConnectionStrings:DevelopmentConnection"])
                .Options;

            Context = new ApplicationDbContext(options);

            _jwtHelper = new(_configuration);
        }

        public string CreateToken(User user)
        {
            Claim[] claims = GenerateClaimsForAccessToken(user);
            TimeSpan expiration = TimeSpan.FromMinutes(5);

            JwtSecurityToken token = _jwtHelper.CreateResuableToken(claims, expiration);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Claim[] GenerateClaimsForAccessToken(User user)
        {
            //Find future data for claims
            string roleName = user.UserAdditionalInfo!.UserRole!.RoleName!;

            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.UserLogin!),
                new Claim(ClaimTypes.Email, user.UserEmail!)
            };
        }

        #region Начальные данные
        protected async Task InitializeTestUser(Guid guid)
        {
            UserRole? userRole = await Context.UserRole.FirstOrDefaultAsync(x => x.RoleName == "admin");
            UserAdditionalInfo userInfo = new()
            {
                IsConfirmedAccount = true,
                UserBalance = 99999999M,
                UserRole = userRole!,
                UserRoleId = userRole!.Id
            };
            User user = new()
            {
                Id = guid,
                UserLogin = "UserUserForTests1",
                UserEmail = "sex@mail.ru",
                PasswordHash = "UserHashForTest1",
                PasswordSalt = "UserSaltForTest1",
                UserAdditionalInfo = userInfo,
            };

            AccessToken = CreateToken(user);
            User = user;
            try
            {
                await Context.User.AddAsync(user);
            }
            catch
            {
                Context.User.Remove(user);
                await Context.User.AddAsync(user);
            }

            await Context.UserAdditionalInfo.AddAsync(userInfo);
            await Context.SaveChangesAsync();
        }

        protected async Task RemoveTestUser(Guid guid)
        {
            User? user = await Context.User.FindAsync(guid);
            Context.User.Remove(user!);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
