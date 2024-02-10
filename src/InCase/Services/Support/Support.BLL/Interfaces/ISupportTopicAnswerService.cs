using Support.BLL.Models;

namespace Support.BLL.Interfaces;
public interface ISupportTopicAnswerService
{
    public Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicAnswerResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
    public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default);
    public Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default);
    public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default);
    public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
}