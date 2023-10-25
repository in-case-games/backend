using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using Payment.DAL.Data;
using Payment.DAL.Entities;

namespace Payment.BLL.Services
{
    public class UserPaymentsService : IUserPaymentsService
    {
        private readonly ApplicationDbContext _context;

        public UserPaymentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPaymentsResponse> GetByIdAsync(Guid id, Guid userId)
        {
            UserPayment payment = await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.Id == id) ?? 
                throw new NotFoundException("Счёт оплаты не найден");

            return payment.UserId == userId ? 
                payment.ToResponse() : 
                throw new ForbiddenException("Счёт оплаты числится на другого пользователя");
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserPayment> payments = await _context.Payments
                .AsNoTracking()
                .OrderByDescending(up => up.Date)
                .Take(count)
                .ToListAsync();

            return payments.ToResponse();
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(Guid userId, int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserPayment> payments = await _context.Payments
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.Date)
                .Take(count)
                .ToListAsync();

            return payments.ToResponse();
        }
    }
}
