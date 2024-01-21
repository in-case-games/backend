using Resources.DAL.Entities;

namespace Resources.BLL.Interfaces;

public interface IGroupLootBoxService
{
    public Task<List<GroupLootBox>> GetAsync(CancellationToken cancellation = default);
    public Task<GroupLootBox> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<GroupLootBox> GetAsync(string name, CancellationToken cancellation = default);
    public Task<GroupLootBox> CreateAsync(GroupLootBox request, CancellationToken cancellation = default);
    public Task<GroupLootBox> UpdateAsync(GroupLootBox request, CancellationToken cancellation = default);
    public Task<GroupLootBox> DeleteAsync(Guid id, CancellationToken cancellation = default);
}
