using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;

namespace Support.BLL.Services
{
    public class AnswerImageService : IAnswerImageService
    {
        private readonly ApplicationDbContext _context;

        public AnswerImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<AnswerImageResponse> GetAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerImageResponse> CreateAsync(Guid userId, AnswerImageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerImageResponse> DeleteAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerImageResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
