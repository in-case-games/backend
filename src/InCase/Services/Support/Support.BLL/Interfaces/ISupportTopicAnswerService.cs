using Support.BLL.Models;

namespace Support.BLL.Interfaces
{
    public interface ISupportTopicAnswerService
    {
        public Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id);
        public Task<SupportTopicAnswerResponse> GetAsync(Guid id);
        public Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId);
        public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid userId, Guid id);
        public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id);
        public Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request);
        public Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request);
        public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request);
        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id);
    }
}
