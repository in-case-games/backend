﻿using Authentication.BLL.Exceptions;
using Authentication.BLL.Helpers;
using Authentication.BLL.Interfaces;
using Authentication.BLL.MassTransit;
using Authentication.BLL.Models;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.Email;
using Infrastructure.MassTransit.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Authentication.BLL.Services
{
    //TODO ButtonLink edit
    public class AuthenticationConfirmService : IAuthenticationConfirmService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly BasePublisher _publisher;

        public AuthenticationConfirmService(
            ApplicationDbContext context, 
            IAuthenticationService authenticationService, 
            IJwtService jwtService,
            BasePublisher publisher)
        {
            _context = context;
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _publisher = publisher;
        }

        public async Task<TokensResponse> ConfirmAccountAsync(string token)
        {
            User user = await _authenticationService.GetUserFromTokenAsync(token, "email");
            UserAdditionalInfo info = user.AdditionalInfo!;
            TokensResponse response = _jwtService.CreateTokenPair(in user);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
            };

            if (!info.IsConfirmed)
            {
                template.Subject = "Добро пожаловать в InCase!";
                template.Header = new()
                {
                    Title = "Конец этапа",
                    Subtitle = "регистрации"
                };
                template.Body = new()
                {
                    Title = $"Добро пожаловать {user.Login!}",
                    Description = $"Мы рады, что вы новый участник нашего проекта. " +
                    $"Надеемся, что вам понравится наша реализация открытия кейсов. " +
                    $"Подарит множество эмоций и новых предметов."
                };

                SiteStatisticsTemplate statisticsTemplate = new() { Users = 1 };

                await _publisher.SendAsync(statisticsTemplate);
            }
            else if (info.DeletionDate != null)
            {
                template.Subject = "Отмена удаления аккаунта";
                template.Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Ваш аккаунт больше не в списках на удаление." +
                    $"Спасибо, что остаётесь с нами!"
                };
            }
            else {
                template.Subject = "Успешный вход в аккаунт";
                template.Header = new()
                {
                    Title = "Конец этапа",
                    Subtitle = "регистрации"
                };
                template.Body = new()
                {
                    Title = $"Добро пожаловать {user.Login!}",
                    Description = $"В ваш аккаунт вошли." +
                    $"Если это были не вы, то срочно измените пароль в настройках вашего аккаунта, " +
                    $"вас автоматически отключит со всех устройств."
                };
            }

            await _publisher.SendAsync(template);

            info.IsConfirmed = true;
            info.DeletionDate = null;

            _context.AdditionalInfos.Update(info);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<UserResponse> DeleteAsync(string token)
        {
            User user = await _authenticationService.GetUserFromTokenAsync(token, "email");

            user.AdditionalInfo!.DeletionDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт будет удален",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}.",
                    Description = $"Ваш аккаунт будет удален в течении 30 дней." +
                    $"Если вы передумали в своем решении просто войдите в аккаунт," +
                    $"и произойдет отмена удаления." +
                    $"Если это не пытались удалить аккаунт срочно поменяйте пароль."
                }
            };

            await _publisher.SendAsync(template);

            _context.AdditionalInfos.Update(user.AdditionalInfo);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateEmailAsync(string email, string token)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User temp = await _authenticationService.GetUserFromTokenAsync(token, "email");

            User user = await _context.Users
                .FirstAsync(u => u.Id == temp.Id);

            EmailTemplate messageOldEmail = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил почту",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Вы изменили email своего аккаунта. " +
                    $"Теперь ваш аккаунт привязан к {email}" +
                    $"Если это были не вы обратитесь в тех поддержку."
                }
            };

            EmailTemplate messageNewEmail = new()
            {
                Email = email,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил почту",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Вы изменили email своего аккаунта." +
                    $"Теперь это почта привязана к вашему аккаунта." +
                    $"Прошлый адрес почты: {user.Email}" +
                    $"Если это были не вы обратитесь в тех поддержку."
                }
            };

            user.Email = email;

            await _context.SaveChangesAsync();

            await _publisher.SendAsync(user.ToTemplate(false));
            await _publisher.SendAsync(messageOldEmail);
            await _publisher.SendAsync(messageNewEmail);

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateEmailByAdminAsync(Guid userId, string email)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email))
                throw new ConflictException("Email почта занята");

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            EmailTemplate messageOldEmail = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил почту",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Администрация сменила вам почту. " +
                    $"Теперь ваш аккаунт привязан к {email}" +
                    $"Если это была ошибка обратитесь в тех. поддержку"
                }
            };

            EmailTemplate messageNewEmail = new()
            {
                Email = email,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил почту",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Администрация сменила вам почту. " +
                    $"Теперь это почта привязана к вашему аккаунта." +
                    $"Прошлый адрес почты: {user.Email}" +
                    $"Если это была ошибка обратитесь в тех. поддержку"
                }
            };

            user.Email = email;

            await _context.SaveChangesAsync();

            await _publisher.SendAsync(user.ToTemplate(false));
            await _publisher.SendAsync(messageOldEmail);
            await _publisher.SendAsync(messageNewEmail);

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateLoginAsync(string login, string token)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Login == login))
                throw new ConflictException("Логин занят");

            User temp = await _authenticationService.GetUserFromTokenAsync(token, "email");

            User user = await _context.Users
                .FirstAsync(u => u.Id == temp.Id);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил логин",
                Body = new()
                {
                    Title = $"Дорогой {login}",
                    Description = $"Вы изменили логин своего аккаунта." +
                    $"Прошлый логин: {user.Login!}" +
                    $"Если это были не вы обратитесь в тех поддержку."
                }
            };

            user.Login = login;

            await _context.SaveChangesAsync();

            await _publisher.SendAsync(user.ToTemplate(false));
            await _publisher.SendAsync(template);

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateLoginByAdminAsync(Guid userId, string login)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Login == login))
                throw new ConflictException("Логин занят");

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил логин",
                Body = new()
                {
                    Title = $"Дорогой {login}",
                    Description = $"Ваш логин изменила администрация." +
                    $"Прошлый логин: {user.Login}." +
                    $"Если это была ошибка обратитесь в тех поддержку."
                }
            };

            user.Login = login;

            await _context.SaveChangesAsync();

            await _publisher.SendAsync(user.ToTemplate(false));
            await _publisher.SendAsync(template);

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdatePasswordAsync(string password, string token)
        {
            User user = await _authenticationService.GetUserFromTokenAsync(token, "email");
            AuthenticationService.CreateNewPassword(ref user, password);

            EmailTemplate template = new()
            {
                Email = user.Email!,
                IsRequiredMessage = true,
                Subject = "Ваш аккаунт сменил пароль",
                Body = new()
                {
                    Title = $"Дорогой {user.Login!}",
                    Description = $"Вы изменили пароль своего аккаунта." +
                    $"Если это были не вы смените пароль," +
                    $"если у вас нет доступа обратитесь в тех поддержку."
                }
            };

            await _publisher.SendAsync(template);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }
    }
}
