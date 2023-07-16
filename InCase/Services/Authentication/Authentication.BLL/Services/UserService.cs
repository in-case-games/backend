using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public UserService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IBus bus)
        {
            _context = context;
            _configuration = configuration;
            _bus = bus;
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

                Uri uri = new(_configuration["MassTransit:Uri"] + "/user");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(user.ToTemplate(true), stoppingToken);
            }

            await _context.SaveChangesAsync(stoppingToken);
        }
    }
}
