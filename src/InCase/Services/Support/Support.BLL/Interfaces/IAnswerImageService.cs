using Support.BLL.Models;

namespace Support.BLL.Interfaces;
public interface IAnswerImageService
{
    public Task<AnswerImageResponse> GetAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<AnswerImageResponse> GetAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid id, CancellationToken cancellation = default);
    public Task<List<AnswerImageResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default);
    public Task<AnswerImageResponse> CreateAsync(Guid userId, AnswerImageRequest request, CancellationToken cancellation = default);
    public Task<AnswerImageResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    public Task<AnswerImageResponse> DeleteAsync(Guid id, CancellationToken cancellation = default);
}