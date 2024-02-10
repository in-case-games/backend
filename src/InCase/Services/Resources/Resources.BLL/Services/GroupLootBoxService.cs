using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Interfaces;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services;
public class GroupLootBoxService(ApplicationDbContext context) : IGroupLootBoxService
{
    public async Task<List<GroupLootBox>> GetAsync(CancellationToken cancellation = default) =>
        await context.GroupLootBoxes
        .AsNoTracking()
        .ToListAsync(cancellation);

    public async Task<GroupLootBox> GetAsync(Guid id, CancellationToken cancellation = default) =>
        await context.GroupLootBoxes
        .AsNoTracking()
        .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ?? 
        throw new NotFoundException("Группа кейсов не найдена");

    public async Task<GroupLootBox> GetAsync(string name, CancellationToken cancellation = default) =>
        await context.GroupLootBoxes
        .AsNoTracking()
        .FirstOrDefaultAsync(glb => glb.Name == name, cancellation) ??
        throw new NotFoundException("Группа кейсов не найдена");

    public async Task<GroupLootBox> CreateAsync(GroupLootBox request, CancellationToken cancellation = default)
    {
        ValidationService.IsGroupLootBox(request);

        request.Id = Guid.NewGuid();

        if (await context.GroupLootBoxes.AnyAsync(glb => glb.Name == request.Name, cancellation))
            throw new ConflictException("Название группы кейсов занято");

        await context.GroupLootBoxes.AddAsync(request, cancellation);
        await context.SaveChangesAsync(cancellation);

        return request;
    }

    public async Task<GroupLootBox> UpdateAsync(GroupLootBox request, CancellationToken cancellation = default)
    {
        ValidationService.IsGroupLootBox(request);

        if (!await context.GroupLootBoxes.AnyAsync(glb => glb.Id == request.Id, cancellation)) 
            throw new NotFoundException("Группа кейсов не найдена");
        if (await context.GroupLootBoxes.AnyAsync(glb => glb.Name == request.Name && glb.Id != request.Id, cancellation))
            throw new ConflictException("Название группы кейсов занято");

        context.GroupLootBoxes.Update(request);
        await context.SaveChangesAsync(cancellation);

        return request;
    }

    public async Task<GroupLootBox> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var group = await context.GroupLootBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ??
            throw new NotFoundException("Группа кейсов не найдена");

        context.GroupLootBoxes.Remove(group);
        await context.SaveChangesAsync(cancellation);

        return group;
    }
}