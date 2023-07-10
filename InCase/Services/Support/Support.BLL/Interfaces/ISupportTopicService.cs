﻿using Support.BLL.Models;

namespace Support.BLL.Interfaces
{
    public interface ISupportTopicService
    {
        public Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id);
        public Task<SupportTopicAnswerResponse> GetAsync(Guid id);
        public Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId);
        public Task<List<SupportTopicAnswerResponse>> GetOpenedTopicsAsync();
        public Task<List<SupportTopicAnswerResponse>> GetAllNoAnswerForSupport();
        public Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request);
        public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request);
        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id);
        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid id);
    }
}
