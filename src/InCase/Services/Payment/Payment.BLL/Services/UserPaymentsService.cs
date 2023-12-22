using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models;
using Payment.DAL.Data;

namespace Payment.BLL.Services
{
    public class UserPaymentsService : IUserPaymentsService
    {
        private readonly ApplicationDbContext _context;

        public UserPaymentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPaymentsResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation = default)
        {
            var payment = await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.Id == id, cancellation) ?? 
                throw new NotFoundException("Счёт оплаты не найден");

            return payment.UserId == userId ? payment.ToResponse() : 
                throw new ForbiddenException("Счёт оплаты числится на другого пользователя");
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(int count, CancellationToken cancellation = default)
        {
            if (count is <= 0 or >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            var payments = await _context.Payments
                .AsNoTracking()
                .OrderByDescending(up => up.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return payments.ToResponse();
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
        {
            if (count is <= 0 or >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            var payments = await _context.Payments
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return payments.ToResponse();
        }
    }
}
