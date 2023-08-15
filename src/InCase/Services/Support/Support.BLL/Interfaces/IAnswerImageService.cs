using Microsoft.AspNetCore.Http;
using Support.BLL.Models;

namespace Support.BLL.Interfaces
{
    public interface IAnswerImageService
    {
        public Task<AnswerImageResponse> GetAsync(Guid userId, Guid id);
        public Task<AnswerImageResponse> GetAsync(Guid id);
        public Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid userId, Guid id);
        public Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid id);
        public Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid userId, Guid id);
        public Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid id);
        public Task<List<AnswerImageResponse>> GetByUserIdAsync(Guid userId);
        public Task<AnswerImageResponse> CreateAsync(Guid userId, AnswerImageRequest request, IFormFile uploadImage);
        public Task<AnswerImageResponse> DeleteAsync(Guid userId, Guid id);
        public Task<AnswerImageResponse> DeleteAsync(Guid id);
    }
}
