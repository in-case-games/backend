using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.MassTransit.Consumers
{
    public class UserRestrictionConsumer : IConsumer<UserRestrictionTemplate>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRestrictionService _restrictionService;

        public UserRestrictionConsumer(
            ApplicationDbContext context, 
            IUserRestrictionService restrictionService)
        {
            _context = context;
            _restrictionService = restrictionService;
        }

        public async Task Consume(ConsumeContext<UserRestrictionTemplate> context)
        {
            UserRestrictionTemplate template = context.Message;

            UserRestriction? restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == template.Id);

            if (restriction is null)
                await _restrictionService.CreateAsync(template.ToRequest());
            else if (template.IsDeleted)
                await _restrictionService.DeleteAsync(template.Id);
            else
                await _restrictionService.UpdateAsync(template.ToRequest());
        }
    }
}
