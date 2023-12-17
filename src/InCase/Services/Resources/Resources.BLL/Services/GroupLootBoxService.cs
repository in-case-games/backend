using Microsoft.EntityFrameworkCore;
using Resources.BLL.Exceptions;
using Resources.BLL.Interfaces;
using Resources.DAL.Data;
using Resources.DAL.Entities;

namespace Resources.BLL.Services
{
    public class GroupLootBoxService : IGroupLootBoxService
    {
        private readonly ApplicationDbContext _context;

        public GroupLootBoxService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupLootBox>> GetAsync(CancellationToken cancellation = default) =>
            await _context.GroupBoxes
            .AsNoTracking()
            .ToListAsync(cancellation);

        public async Task<GroupLootBox> GetAsync(Guid id, CancellationToken cancellation = default) =>
            await _context.GroupBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ?? 
            throw new NotFoundException("Группа кейсов не найдена");

        public async Task<GroupLootBox> GetAsync(string name, CancellationToken cancellation = default) =>
            await _context.GroupBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Name == name, cancellation) ??
            throw new NotFoundException("Группа кейсов не найдена");

        public async Task<GroupLootBox> CreateAsync(GroupLootBox request, CancellationToken cancellation = default)
        {
            request.Id = Guid.NewGuid();

            if (await _context.GroupBoxes.AnyAsync(glb => glb.Name == request.Name, cancellation))
                throw new ConflictException("Название группы кейсов занято");

            await _context.GroupBoxes.AddAsync(request, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return request;
        }

        public async Task<GroupLootBox> UpdateAsync(GroupLootBox request, CancellationToken cancellation = default)
        {
            var group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(glb => glb.Id == request.Id, cancellation) ??
                throw new NotFoundException("Группа кейсов не найдена");

            if (await _context.GroupBoxes
                .AnyAsync(glb => glb.Name == request.Name && glb.Id != request.Id, cancellation))
                throw new ConflictException("Название группы кейсов занято");

            _context.GroupBoxes.Update(request);
            await _context.SaveChangesAsync(cancellation);

            return request;
        }

        public async Task<GroupLootBox> DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(glb => glb.Id == id, cancellation) ??
                throw new NotFoundException("Группа кейсов не найдена");

            _context.GroupBoxes.Remove(group);
            await _context.SaveChangesAsync(cancellation);

            return group;
        }
    }
}
