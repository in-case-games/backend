using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Authentication.BLL.Exceptions;

namespace Authentication.BLL.Services
{
    public class AuthenticationSendingService : IAuthenticationSendingService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public AuthenticationSendingService(
            IAuthenticationService authenticationService, 
            IJwtService jwtService,
            ApplicationDbContext context)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task ConfirmAccountAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || request.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            request.Email = user.Email!;

            MapDataMailRequest(ref request, in user);

            if(user.AdditionalInfo!.IsConfirmed)
            {
                //TODO Notify rabbit mq email sender
            }
            else
            {
                //TODO Notify rabbit mq email sender
            }
        }

        public async Task ConfirmNewEmailAsync(DataMailRequest request, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User user = await _authenticationService.GetUserFromTokenAsync(request.Token, "email");

            MapDataMailRequest(ref request, in user);

            request.Email = email;

            //TODO Notify rabbit mq email sender
        }

        public async Task DeleteAccountAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            //TODO Notify rabbit mq email sender
        }

        public async Task ForgotPasswordAsync(DataMailRequest request)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            MapDataMailRequest(ref request, in user);

            request.Email = user.Email!;

            //TODO Notify rabbit mq email sender
        }

        public async Task UpdateEmailAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            //TODO Notify rabbit mq email sender
        }

        public async Task UpdatePasswordAsync(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login || u.Email == request.Email) ??
                throw new NotFoundException("Пользователь не найден");

            if (!ValidationService.IsValidUserPassword(in user, password))
                throw new ForbiddenException("Неверный пароль");

            MapDataMailRequest(ref request, in user);

            //TODO Notify rabbit mq email sender
        }

        private void MapDataMailRequest(ref DataMailRequest request, in User user)
        {
            request.Email = user.Email!;
            request.Login = user.Login!;
            request.Token = _jwtService.CreateEmailToken(user);
        }
    }
}
