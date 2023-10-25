using Support.BLL.Models;

namespace Support.BLL.Interfaces
{
    public interface ISupportTopicService
    {
        public Task<SupportTopicResponse> GetAsync(Guid userId, Guid id);
        public Task<SupportTopicResponse> GetAsync(Guid id);
        public Task<List<SupportTopicResponse>> GetByUserIdAsync(Guid userId);
        public Task<List<SupportTopicResponse>> GetOpenedTopicsAsync();
        public Task<SupportTopicResponse> CloseTopic(Guid id);
        public Task<SupportTopicResponse> CloseTopic(Guid userId, Guid id);
        public Task<SupportTopicResponse> CreateAsync(SupportTopicRequest request);
        public Task<SupportTopicResponse> UpdateAsync(SupportTopicRequest request);
        public Task<SupportTopicResponse> DeleteAsync(Guid userId, Guid id);
        public Task<SupportTopicResponse> DeleteAsync(Guid id);
    }
}
