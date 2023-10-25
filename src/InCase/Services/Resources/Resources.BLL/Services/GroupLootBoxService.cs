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

        public async Task<List<GroupLootBox>> GetAsync() =>
            await _context.GroupBoxes
            .AsNoTracking()
            .ToListAsync();

        public async Task<GroupLootBox> GetAsync(Guid id) =>
            await _context.GroupBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Id == id) ?? 
            throw new NotFoundException("Группа кейсов не найдена");

        public async Task<GroupLootBox> GetAsync(string name) =>
            await _context.GroupBoxes
            .AsNoTracking()
            .FirstOrDefaultAsync(glb => glb.Name == name) ??
            throw new NotFoundException("Группа кейсов не найдена");

        public async Task<GroupLootBox> CreateAsync(GroupLootBox request)
        {
            request.Id = Guid.NewGuid();

            if (await _context.GroupBoxes.AnyAsync(glb => glb.Name == request.Name))
                throw new ConflictException("Название группы кейсов занято");

            await _context.GroupBoxes.AddAsync(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<GroupLootBox> UpdateAsync(GroupLootBox request)
        {
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(glb => glb.Id == request.Id) ??
                throw new NotFoundException("Группа кейсов не найдена");

            if (await _context.GroupBoxes
                .AnyAsync(glb => glb.Name == request.Name && glb.Id != request.Id))
                throw new ConflictException("Название группы кейсов занято");

            _context.GroupBoxes.Update(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<GroupLootBox> DeleteAsync(Guid id)
        {
            GroupLootBox group = await _context.GroupBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(glb => glb.Id == id) ??
                throw new NotFoundException("Группа кейсов не найдена");

            _context.GroupBoxes.Remove(group);
            await _context.SaveChangesAsync();

            return group;
        }
    }
}
