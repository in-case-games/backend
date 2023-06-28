using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
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

        public async Task ConfirmAccount(DataMailRequest request, string password)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == request.Login) ??
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

        public async Task ConfirmNewEmail(DataMailRequest request, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User user = await _authenticationService.GetUserFromToken(request.Token, "email");

            MapDataMailRequest(ref request, in user);

            request.Email = email;

            //TODO Notify rabbit mq email sender
        }

        public Task DeleteAccount(DataMailRequest request, string password)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(DataMailRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEmail(DataMailRequest request, string password)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePassword(DataMailRequest request, string password)
        {
            throw new NotImplementedException();
        }

        private void MapDataMailRequest(ref DataMailRequest request, in User user)
        {
            request.Email = user.Email!;
            request.Login = user.Login!;
            request.Token = _jwtService.CreateEmailToken(user);
        }
    }
}
