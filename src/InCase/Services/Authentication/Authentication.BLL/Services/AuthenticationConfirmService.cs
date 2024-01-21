using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Infrastructure.MassTransit.Email;
using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services;

public class AuthenticationConfirmService(
    ApplicationDbContext context, 
    IAuthenticationService authenticationService, 
    IJwtService jwtService, 
    BasePublisher publisher) : IAuthenticationConfirmService
{
    public async Task<TokensResponse> ConfirmAccountAsync(string token, CancellationToken cancellationToken = default)
    {
        var user = await authenticationService.GetUserFromTokenAsync(token, "email", cancellationToken);
        var tokenPair = jwtService.CreateTokenPair(in user);
        var email = new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
        };
        var stats = new SiteStatisticsTemplate { Users = 0 };

        if (!user.AdditionalInfo!.IsConfirmed)
        {
            email.Subject = "Добро пожаловать в InCase!";
            email.Header = new EmailHeaderTemplate
            {
                Title = "Конец этапа",
                Subtitle = "регистрации"
            };
            email.Body = new EmailBodyTemplate
            {
                Title = $"Добро пожаловать {user.Login!}",
                Description = $"Мы рады, что вы новый участник нашего проекта. " +
                $"Надеемся, что вам понравится наша реализация открытия кейсов. " +
                $"Подарит множество эмоций и новых предметов."
            };

            stats.Users = 1;
        }
        else if (user.AdditionalInfo.DeletionDate != null)
        {
            email.Subject = "Отмена удаления аккаунта";
            email.Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Ваш аккаунт больше не в списках на удаление." +
                $"Спасибо, что остаётесь с нами!"
            };
        }
        else {
            email.Subject = "Успешный вход в аккаунт";
            email.Header = new EmailHeaderTemplate
            {
                Title = "Конец этапа",
                Subtitle = "регистрации"
            };
            email.Body = new EmailBodyTemplate
            {
                Title = $"Добро пожаловать {user.Login!}",
                Description = $"В ваш аккаунт вошли." +
                $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                $"вас автоматически отключит со всех устройств."
            };
        }

        user.AdditionalInfo.IsConfirmed = true;
        user.AdditionalInfo.DeletionDate = null;

        context.AdditionalInfos.Update(user.AdditionalInfo);
        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(email, cancellationToken);

        if(stats.Users == 1)  await publisher.SendAsync(stats, cancellationToken);

        return tokenPair;
    }

    public async Task<UserResponse> DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var user = await authenticationService.GetUserFromTokenAsync(token, "email", cancellationToken);
        
        user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

        context.AdditionalInfos.Update(user.AdditionalInfo);
        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт будет удален",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}.",
                Description = $"Ваш аккаунт будет удален в течении 30 дней." +
                              $"Если вы передумали в своем решении просто войдите в аккаунт," +
                              $"и произойдет отмена удаления." +
                              $"Если это не пытались удалить аккаунт срочно поменяйте пароль."
            }
        }, cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        if (!ValidationService.CheckCorrectEmail(email)) 
            throw new BadRequestException("Некорректный mail");
        if (await context.Users.AsNoTracking().AnyAsync(u => u.Email == email, cancellationToken))
            throw new ConflictException("Email почта занята");

        var userFromToken = await authenticationService.GetUserFromTokenAsync(token, "email", cancellationToken);
        var user = await context.Users.FirstAsync(u => u.Id == userFromToken.Id, cancellationToken);
        var oldEmail = user.Email!;

        user.Email = email;

        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(user.ToTemplate(false), cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил почту",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Вы изменили email своего аккаунта. " +
                $"Теперь ваш аккаунт привязан к {email}." +
                $"Если это были не вы обратитесь в тех поддержку."
            }
        }, cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = oldEmail,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил почту",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Вы изменили email своего аккаунта." +
                $"Теперь это почта привязана к вашему аккаунта." +
                $"Прошлый адрес почты: {user.Email}." +
                $"Если это были не вы обратитесь в тех поддержку."
            }
        }, cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateEmailByAdminAsync(Guid userId, string email, CancellationToken cancellationToken = default)
    {
        if (!ValidationService.CheckCorrectEmail(email)) 
            throw new BadRequestException("Некорректный mail");
        if (await context.Users.AsNoTracking().AnyAsync(u => u.Email == email, cancellationToken))
            throw new ConflictException("Email почта занята");

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) ??
            throw new NotFoundException("Пользователь не найден");

        user.Email = email;

        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(user.ToTemplate(false), cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил почту",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Администрация сменила вам почту. " +
                $"Теперь ваш аккаунт привязан к {email}" +
                $"Если это была ошибка обратитесь в тех. поддержку"
            }
        }, cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = email,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил почту",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Администрация сменила вам почту. " +
                $"Теперь это почта привязана к вашему аккаунта." +
                $"Прошлый адрес почты: {user.Email}" +
                $"Если это была ошибка обратитесь в тех. поддержку"
            }
        }, cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateLoginAsync(string login, string token, CancellationToken cancellationToken)
    {
        if (!ValidationService.CheckCorrectLogin(login)) 
            throw new BadRequestException("Некорректный логин");
        if (await context.Users.AsNoTracking().AnyAsync(u => u.Login == login, cancellationToken))
            throw new ConflictException("Логин занят");

        var userFromToken = await authenticationService.GetUserFromTokenAsync(token, "email", cancellationToken);
        var user = await context.Users.FirstAsync(u => u.Id == userFromToken.Id, cancellationToken);

        user.Login = login;

        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(user.ToTemplate(false), cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил логин",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {login}",
                Description = $"Вы изменили логин своего аккаунта." +
                $"Прошлый логин: {user.Login!}" +
                $"Если это были не вы обратитесь в тех поддержку."
            }
        }, cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateLoginByAdminAsync(Guid userId, string login, CancellationToken cancellationToken = default)
    {
        if (!ValidationService.CheckCorrectLogin(login))
            throw new BadRequestException("Некорректный логин");
        if (await context.Users.AsNoTracking().AnyAsync(u => u.Login == login, cancellationToken))
            throw new ConflictException("Логин занят");

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken) ??
            throw new NotFoundException("Пользователь не найден");

        user.Login = login;

        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(user.ToTemplate(false), cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил логин",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {login}",
                Description = $"Ваш логин изменила администрация." +
                $"Прошлый логин: {user.Login}." +
                $"Если это была ошибка обратитесь в тех поддержку."
            }
        }, cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdatePasswordAsync(string password, string token, CancellationToken cancellationToken = default)
    {
        var user = await authenticationService.GetUserFromTokenAsync(token, "email", cancellationToken);

        AuthenticationService.CreateNewPassword(ref user, password);

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        await publisher.SendAsync(new EmailTemplate
        {
            Email = user.Email!,
            IsRequiredMessage = true,
            Subject = "Ваш аккаунт сменил пароль",
            Body = new EmailBodyTemplate
            {
                Title = $"Дорогой {user.Login!}",
                Description = $"Вы изменили пароль своего аккаунта." +
                $"Если это были не вы смените пароль," +
                $"если у вас нет доступа обратитесь в тех поддержку."
            }
        }, cancellationToken);

        return user.ToResponse();
    }
}