using Infrastructure.MassTransit.User;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.BLL.Interfaces;
using Payment.DAL.Data;

namespace Payment.BLL.MassTransit.Consumers;
public class UserConsumer(IUserService userService, ApplicationDbContext context) : IConsumer<UserTemplate>
{
    public async Task Consume(ConsumeContext<UserTemplate> context1)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == context1.Message.Id);

        if (user is null) await userService.CreateAsync(context1.Message);
        else if (context1.Message.IsDeleted) await userService.DeleteAsync(user.Id);
    }
}