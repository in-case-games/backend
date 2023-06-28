using Authentication.BLL.Exceptions;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthenticationService(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task SignIn(UserRequest request)
        {
            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                u.Id == request.Id || u.Email == request.Email || u.Login == request.Login) ??
                throw new NotFoundException("Пользователь не найден");

            if(!ValidationService.IsValidUserPassword(in user, request.Password))
                throw new ForbiddenException("Неверный пароль");

            
        }

        public Task SignUp(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TokensResponse> RefreshTokens(string token)
        {
            throw new NotImplementedException();
        }
    }
}
