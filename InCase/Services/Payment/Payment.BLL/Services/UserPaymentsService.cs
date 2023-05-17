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
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public UserPaymentsService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserPaymentsResponse> GetByIdAsync(Guid id, Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserPayments payment = await context.UserPayments
                .Include(up => up.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.Id == id) ?? 
                throw new NotFoundException("Счёт оплаты не найден");

            if (payment.UserId != userId)
                throw new ForbiddenException("Счёт оплаты числится на другого пользователя");

            return payment.ToResponse();
        }

        public async Task<List<UserPaymentsResponse>> GetAsync(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserPayments> payments = await context.UserPayments
                .Include(up => up.Status)
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .ToListAsync();

            return payments.ToResponse();
        }

        public async Task<List<UserPaymentsResponse>> GetAsync()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserPayments> payments = await context.UserPayments
                .Include(up => up.Status)
                .AsNoTracking()
                .ToListAsync();

            return payments.ToResponse();
        }
    }
}
