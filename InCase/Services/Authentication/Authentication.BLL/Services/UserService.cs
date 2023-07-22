using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public UserService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task DoWorkManagerAsync(CancellationToken stoppingToken)
        {
            List<User> users = await _context.Users
                .Include(u => u.AdditionalInfo)
                .AsNoTracking()
                .Where(uai => uai.AdditionalInfo!.DeletionDate <= DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            foreach (var user in users)
            {
                _context.Users.Remove(user);
                await _publisher.SendAsync(user.ToTemplate(true), "/user");
            }

            await _context.SaveChangesAsync(stoppingToken);
        }
    }
}
