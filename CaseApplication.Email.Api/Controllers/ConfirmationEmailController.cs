﻿using CaseApplication.Domain.Entities.External;
using CaseApplication.Domain.Entities.Internal;
using CaseApplication.Infrastructure.Data;
using CaseApplication.Infrastructure.Helpers;
using CaseApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaseApplication.Email.Api.Controllers
{
    [Route("email/api/[controller]")]
    [ApiController]
    public class ConfirmationEmailController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly ValidationService _validationService;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ConfirmationEmailController(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            EmailHelper emailHelper,
            JwtHelper jwtHelper,
            ValidationService validationService)
        {
            _contextFactory = contextFactory;
            _emailHelper = emailHelper;
            _jwtHelper = jwtHelper;
            _validationService = validationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendConfirm(EmailPattern emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid();

            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);

            await _emailHelper.SendConfirmAccountToEmail(emailModel);

            return Accepted();
        }

        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> SendConfirmForgotPassword(EmailPattern emailModel)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null) return NotFound();
            if (user.UserEmail != emailModel.UserEmail) return Forbid();

            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);

            await _emailHelper.SendChangePasswordToEmail(emailModel);

            return Accepted();
        }

        [Authorize]
        [HttpPut("email/{password}")]
        public async Task<IActionResult> SendConfirmUpdateEmail(EmailPattern emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return Accepted();
        }
        
        [Authorize]
        [HttpPut("password/{password}")]
        public async Task<IActionResult> SendConfirmUpdatePassword(EmailPattern emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return Accepted();
        }

        [Authorize]
        [HttpDelete("{password}")]
        public async Task<IActionResult> SendConfirmDeleteAccount(EmailPattern emailModel, string password)
        {
            try
            {
                await Send(emailModel, password);
            }
            catch (ArgumentException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }

            return Accepted();
        }

        private async Task Send(EmailPattern emailModel, string password)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            User? user = await context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
                throw new NullReferenceException("The user was not found");
            if (!_validationService.IsValidUserPassword(in user, password))
                throw new ArgumentException("Invalid data");

            MapEmailModelForSend(ref emailModel, user);
            await _emailHelper.SendDeleteAccountToEmail(emailModel);
        }

        private void MapEmailModelForSend(ref EmailPattern emailModel, User user)
        {
            emailModel.UserEmail = user.UserEmail!;
            emailModel.UserId = user.Id;
            emailModel.EmailToken = _jwtHelper.GenerateEmailToken(user);
        }
    }
}