using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.MassTransit;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services;

public class UserRestrictionService(ApplicationDbContext context, BasePublisher publisher) : IUserRestrictionService
{
    public async Task<UserRestrictionResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var restriction = await context.Restrictions
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Эффект не найден");

        return restriction.ToResponse();
    }

    public async Task<List<UserRestrictionResponse>> GetAsync(Guid userId, Guid ownerId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Обвиняемый не найден");
        if (!await context.Users.AnyAsync(u => u.Id == ownerId, cancellation))
            throw new NotFoundException("Обвинитель не найден");

        var restrictions = await context.Restrictions
            .Include(ur => ur.Type)
            .AsNoTracking()
            .Where(ur => ur.OwnerId == ownerId && ur.UserId == userId)
            .ToListAsync(cancellation);

        return restrictions.ToResponse();
    }

    public async Task<List<UserRestrictionResponse>> GetByLoginAsync(string login, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .Include(u => u.Restrictions!)
                .ThenInclude(ur => ur.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        return user.Restrictions?.ToResponse() ?? [];
    }

    public async Task<List<UserRestrictionResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .Include(u => u.Restrictions!)
                .ThenInclude(ur => ur.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        return user.Restrictions?.ToResponse() ?? [];
    }

    public async Task<List<UserRestrictionResponse>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellation = default)
    {
        var user = await context.Users
            .Include(u => u.OwnerRestrictions!)
                .ThenInclude(ur => ur.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == ownerId, cancellation) ??
            throw new NotFoundException("Пользователь не найден");

        return user.OwnerRestrictions?.ToResponse() ?? [];
    }

    public async Task<List<RestrictionTypeResponse>> GetTypesAsync(CancellationToken cancellation = default)
    {
        var types = await context.RestrictionTypes
            .AsNoTracking()
            .ToListAsync(cancellation);

        return types.ToResponse();
    }

    public async Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request, CancellationToken cancellation = default)
    {
        var type = await context.RestrictionTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == request.TypeId, cancellation) ??
            throw new NotFoundException("Тип эффекта не найден");
        var user = await context.Users
            .Include(u => u.AdditionalInfo)
            .Include(u => u.AdditionalInfo!.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellation) ??
            throw new NotFoundException("Обвиняемый не найден");
        var owner = await context.Users
            .Include(u => u.AdditionalInfo)
            .Include(u => u.AdditionalInfo!.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.OwnerId, cancellation) ??
            throw new NotFoundException("Обвинитель не найден");

        request = await CheckUserRestriction(request, type, cancellation);

        var restriction = request.ToEntity(isNewGuid: true);
        var userRole = user.AdditionalInfo!.Role!.Name!;

        if (userRole != "user") throw new ForbiddenException("Эффект можно наложить только на пользователя");

        await context.Restrictions.AddAsync(restriction, cancellation);
        await context.SaveChangesAsync(cancellation);

        if (request.TypeId != type.Id)
        {
            type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstAsync(rt => rt.Id == request.TypeId, cancellation);
        }

        restriction.Type = type;

        if (restriction.Type.Name == "ban") await publisher.SendAsync(restriction.ToTemplate(), cancellation);

        return restriction.ToResponse();
    }

    public async Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request, CancellationToken cancellation = default)
    {
        var type = await context.RestrictionTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Id == request.TypeId, cancellation) ??
            throw new NotFoundException("Тип эффекта не найден");
        var user = await context.Users
            .Include(u => u.AdditionalInfo)
            .Include(u => u.AdditionalInfo!.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellation) ??
            throw new NotFoundException("Обвиняемый не найден");
        var owner = await context.Users
            .Include(u => u.AdditionalInfo)
            .Include(u => u.AdditionalInfo!.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.OwnerId, cancellation) ??
            throw new NotFoundException("Обвинитель не найден");
        var restrictionOld = await context.Restrictions
            .Include(ur => ur.Type)
            .FirstOrDefaultAsync(ur => ur.Id == request.Id, cancellation) ??
            throw new NotFoundException("Эффект не найден");

        request = await CheckUserRestriction(request, type, cancellation);

        var restriction = request.ToEntity(isNewGuid: false);
        var userRole = user.AdditionalInfo!.Role!.Name!;

        if (userRole != "user") throw new ForbiddenException("Эффект можно наложить только на пользователя");

        context.Entry(restrictionOld).CurrentValues.SetValues(restriction);
        await context.SaveChangesAsync(cancellation);

        if(request.TypeId != type.Id)
        {
            type = await context.RestrictionTypes
                .AsNoTracking()
                .FirstAsync(rt => rt.Id == request.TypeId, cancellation);
        }

        restriction.Type = type;

        if(restriction.Type.Name == "ban") await publisher.SendAsync(restriction.ToTemplate(), cancellation);

        return restriction.ToResponse();
    }

    public async Task<UserRestrictionResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var restriction = await context.Restrictions
            .Include(ur => ur.Type)
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.Id == id, cancellation) ??
            throw new NotFoundException("Эффект не найден");

        context.Restrictions.Remove(restriction);
        await context.SaveChangesAsync(cancellation);
        await publisher.SendAsync(restriction.ToTemplate(isDeleted: true), cancellation);

        return restriction.ToResponse();
    }

    private async Task<UserRestrictionRequest> CheckUserRestriction(
        UserRestrictionRequest request,
        RestrictionType type,
        CancellationToken cancellation = default)
    {
        var restrictions = await context.Restrictions
            .Include(ur => ur.Type)
            .AsNoTracking()
            .Where(ur => ur.UserId == request.UserId)
            .ToListAsync(cancellation);

        var numberWarns = (type.Name == "warn" ? 1 : 0) + 
                          restrictions.Count(restriction => restriction.Type!.Name == "warn" && restriction.Id != request.Id);

        if (numberWarns < 3) return request;

        var ban = await context.RestrictionTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Name == "ban", cancellation);

        request.TypeId = ban!.Id;
        request.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);

        return request;
    }
}