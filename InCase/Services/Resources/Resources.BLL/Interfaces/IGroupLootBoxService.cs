using Resources.DAL.Entities;

namespace Resources.BLL.Interfaces
{
    public interface IGroupLootBoxService
    {
        public Task<List<GroupLootBox>> GetAsync();
        public Task<GroupLootBox> GetAsync(Guid id);
        public Task<GroupLootBox> GetAsync(string name);
        public Task<GroupLootBox> CreateAsync(GroupLootBox request);
        public Task<GroupLootBox> UpdateAsync(GroupLootBox request);
        public Task<GroupLootBox> DeleteAsync(Guid id);
    }
}
