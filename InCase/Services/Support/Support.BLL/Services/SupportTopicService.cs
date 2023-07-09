using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;

namespace Support.BLL.Services
{
    public class SupportTopicService : ISupportTopicService
    {
        private readonly ApplicationDbContext _context;

        public SupportTopicService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportTopicAnswerResponse>> GetOpenedTopicsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
