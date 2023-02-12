﻿using AutoMapper;
using CaseApplication.Api.Models;
using CaseApplication.Api.Services;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.EntityFramework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EncryptorHelper _encryptorHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly EmailHelper _emailHelper;
        private readonly ValidationService _validationService;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<User, UserDto>();
        });
        private readonly MapperConfiguration _mapperConfigurationInfo = new(configuration =>
        {
            configuration.CreateMap<UserAdditionalInfo, UserAdditionalInfoDto>();
        });
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public UserController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EncryptorHelper encryptorHelper,
            JwtHelper jwtHelper,
            EmailHelper emailHelper,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _encryptorHelper = encryptorHelper;
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
            _validationService = validationService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid? userId = null)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            userId ??= UserId;

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("login/{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserLogin == login);

            if (user == null) return NotFound();
            
            user.PasswordHash = "access denied";
            user.PasswordSalt = "access denied";
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<User> users = await context.User
                .Include(x => x.UserAdditionalInfo)
                .Include(x => x.UserAdditionalInfo!.UserRole)
                .Include(x => x.UserInventories)
                .Include(x => x.PromocodesUsedByUsers)
                .Include(x => x.UserRestrictions)
                .Include(x => x.UserHistoryOpeningCases)
                .AsNoTracking()
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                users[i].UserEmail = "access denied";
                users[i].PasswordHash = "access denied";
                users[i].PasswordSalt = "access denied";
            }

            //TODO Testing speed
/*            foreach (User user in users)
            {
                user.UserEmail = "access denied";
                user.PasswordHash = "access denied";
                user.PasswordSalt = "access denied";
            }*/

            return Ok(users);
        }

        [Authorize]
        [HttpPut("login/{login}")]
        public async Task<IActionResult> UpdateLogin(string login)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUserByLogin = await context.User
                .FirstOrDefaultAsync(x => x.UserLogin == login);

            User? searchUserById = await context.User
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (searchUserByLogin != null) return BadRequest();
            if (searchUserById == null) return NotFound();

            searchUserById.UserLogin = login;

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                searchUserById.UserEmail!, 
                "Администрация сайта" , 
                new EmailPatternModel()
                {
                    Body = $"Имя вашего акканута измененно на: {login}"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail(EmailModel emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? searchUser = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserEmail == emailModel.UserEmail);
            User? user = await context.User
                .Include(x => x.UserAdditionalInfo)
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (searchUser != null) return Forbid("Email is already busy");
            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);
            if (isValidToken is false) return Forbid("Invalid email token");

            user.UserEmail = emailModel.UserEmail;
            user.UserAdditionalInfo!.IsConfirmedAccount = false;

            List<UserToken> userTokens = await context.UserToken
                .AsNoTracking()
                .Where(x => x.UserId == emailModel.UserId)
                .ToListAsync();

            context.UserToken.RemoveRange(userTokens);

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                emailModel.UserEmail,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вы изменили email аккаунта"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> UpdatePasswordConfirmation(EmailModel emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            //Gen hash and salt
            byte[] salt = _encryptorHelper.GenerationSaltTo64Bytes();
            string hash = _encryptorHelper.EncryptorPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = Convert.ToBase64String(salt);

            List<UserToken> userTokens = await context.UserToken
                .AsNoTracking()
                .Where(x => x.UserId == emailModel.UserId)
                .ToListAsync();

            context.UserToken.RemoveRange(userTokens);

            await context.SaveChangesAsync();

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Вы сменили пароль"
                });

            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmation(EmailModel emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == emailModel.UserId);

            if (user == null) return NotFound();

            bool isValidToken = _validationService.IsValidEmailToken(emailModel, user.PasswordHash!);

            if (isValidToken is false) return Forbid("Invalid email token");

            await _emailHelper.SendNotifyToEmail(
                user.UserEmail!,
                "Администрация сайта",
                new EmailPatternModel()
                {
                    Body = $"Ваш аккаунт будет удален через 30 дней"
                });

            //TODO No delete give the user 30 days

            List<UserToken> userTokens = await context.UserToken
                .AsNoTracking()
                .Where(x => x.UserId == emailModel.UserId)
                .ToListAsync();

            context.UserToken.RemoveRange(userTokens);

            await context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{userId}")]
        public async Task<IActionResult> DeleteByAdmin(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null) return NotFound();

            context.User.Remove(user);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
