using Microsoft.EntityFrameworkCore;
using Payment.BLL.Exceptions;
using Payment.BLL.Helpers;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.Internal;
using Payment.DAL.Data;

namespace Payment.BLL.Services;
public class UserPaymentsService(ApplicationDbContext context) : IUserPaymentsService
{
    public async Task<UserPaymentResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation = default)
    {
        var payment = await context.UserPayments
            .AsNoTracking()
            .Include(p => p.Status)
            .FirstOrDefaultAsync(up => up.Id == id, cancellation) ?? 
            throw new NotFoundException("Счёт оплаты не найден");

        return payment.UserId == userId ? payment.ToResponse() : 
            throw new ForbiddenException("Счёт оплаты числится на другого пользователя");
    }

    public async Task<List<UserPaymentResponse>> GetAsync(int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

        var payments = await context.UserPayments
            .AsNoTracking()
            .OrderByDescending(up => up.CreatedAt)
            .Take(count)
            .Include(p => p.Status)
            .ToListAsync(cancellation);

        return payments.ToResponse();
    }

    public async Task<List<UserPaymentResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
    {
        if (count is <= 0 or >= 10000)
            throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

        var payments = await context.UserPayments
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.CreatedAt)
            .Take(count)
            .Include(p => p.Status)
            .ToListAsync(cancellation);

        return payments.ToResponse();
    }
}