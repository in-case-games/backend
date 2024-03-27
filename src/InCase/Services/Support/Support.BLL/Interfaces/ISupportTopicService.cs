using Support.BLL.Models;

namespace Support.BLL.Interfaces;
public interface ISupportTopicService
{
    public Task<SupportTopicResponse> GetAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<SupportTopicResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
    public Task<List<SupportTopicResponse>> GetOpenedTopicsAsync(CancellationToken cancellation = default);
    public Task<SupportTopicResponse> CloseTopic(Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> CloseTopic(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> CreateAsync(SupportTopicRequest request, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> UpdateAsync(SupportTopicRequest request, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<SupportTopicResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}