using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class AuthenticationConfirmService : IAuthenticationConfirmService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;

        public AuthenticationConfirmService(
            ApplicationDbContext context, 
            IAuthenticationService authenticationService, 
            IJwtService jwtService)
        {
            _context = context;
            _authenticationService = authenticationService;
            _jwtService = jwtService;
        }

        public async Task<TokensResponse> ConfirmAccount(string token)
        {
            User user = await _authenticationService.GetUserFromToken(token, "email");
            UserAdditionalInfo info = user.AdditionalInfo!;
            TokensResponse response = _jwtService.CreateTokenPair(in user);

            if (!info.IsConfirmed)
            {
                //TODO Notify rabbit mq email sender
            }
            else if (info.DeletionDate != null)
            {
                //TODO Notify rabbit mq email sender
            }
            else {
                //TODO Notify rabbit mq email sender
            }

            info.IsConfirmed = true;
            info.DeletionDate = null;

            _context.AdditionalInfos.Update(info);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<UserResponse> Delete(string token)
        {
            User user = await _authenticationService.GetUserFromToken(token, "email");

            user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            _context.AdditionalInfos.Update(user.AdditionalInfo);
            await _context.SaveChangesAsync();

            //TODO Notify rabbit mq email sender

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateEmail(string email, string token)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User user = await _authenticationService.GetUserFromToken(token, "email");

            user.Email = email;

            await _context.SaveChangesAsync();
            //TODO Notify rabbit mq email sender

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdatePassword(string password, string token)
        {
            User user = await _authenticationService.GetUserFromToken(token, "email");
            AuthenticationService.CreateNewPassword(ref user, password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            //TODO Notify rabbit mq email sender

            return user.ToResponse();
        }
    }
}
