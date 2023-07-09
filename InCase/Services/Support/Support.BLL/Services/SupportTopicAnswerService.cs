using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;

namespace Support.BLL.Services
{
    public class SupportTopicAnswerService : ISupportTopicAnswerService
    {
        private readonly ApplicationDbContext _context;

        public SupportTopicAnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<SupportTopicAnswerResponse>> GetAllNoAnswerForSupport()
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportTopicAnswerResponse>> GetAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> UpdateByAdminAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
