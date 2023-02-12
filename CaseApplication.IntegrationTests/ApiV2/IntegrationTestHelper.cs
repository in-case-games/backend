﻿using CaseApplication.Api.Services;
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
    }
}
