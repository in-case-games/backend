using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Interfaces;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services;

public class GroupLootBoxService(ApplicationDbContext context) : IGroupLootBoxService
{
    public async Task<List<GroupLootBox>> GetAsync(CancellationToken cancellation = default) =>
        await context.GroupBoxes
        .AsNoTracking()
        .ToListAsync(cancellation);

    public async Task<GroupLootBox> GetAsync(Guid id, CancellationToken cancellation = default) =>
        await context.GroupBoxes
        .AsNoTracking()
        .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ?? 
        throw new NotFoundException("Группа кейсов не найдена");

    public async Task<GroupLootBox> GetAsync(string name, CancellationToken cancellation = default) =>
        await context.GroupBoxes
        .AsNoTracking()
        .FirstOrDefaultAsync(glb => glb.Name == name, cancellation) ??
        throw new NotFoundException("Группа кейсов не найдена");

    public async Task<GroupLootBox> CreateAsync(GroupLootBox request, CancellationToken cancellation = default)
    {
        ValidationService.IsGroupLootBox(request);

        request.Id = Guid.NewGuid();

        if (await context.GroupBoxes.AnyAsync(glb => glb.Name == request.Name, cancellation))
            throw new ConflictException("Название группы кейсов занято");

        await context.GroupBoxes.AddAsync(request, cancellation);
        await context.SaveChangesAsync(cancellation);

        return request;
    }

    public async Task<GroupLootBox> UpdateAsync(GroupLootBox request, CancellationToken cancellation = default)
    {
        ValidationService.IsGroupLootBox(request);

        if (!await context.GroupBoxes.AnyAsync(glb => glb.Id == request.Id, cancellation)) 
            throw new NotFoundException("Группа кейсов не найдена");
        if (await context.GroupBoxes.AnyAsync(glb => glb.Name == request.Name && glb.Id != request.Id, cancellation))
            throw new ConflictException("Название группы кейсов занято");

        context.GroupBoxes.Update(request);
        await context.SaveChangesAsync(cancellation);

        return request;
    }

    public async Task<GroupLootBox> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var group = await context.GroupBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ??
            throw new NotFoundException("Группа кейсов не найдена");

        context.GroupBoxes.Remove(group);
        await context.SaveChangesAsync(cancellation);

        return group;
    }
}