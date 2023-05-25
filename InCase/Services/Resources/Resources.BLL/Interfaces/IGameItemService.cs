﻿using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task<GameItemResponse> GetAsync(Guid id);
        public Task<List<GameItemResponse>> GetAsync();
        public Task<List<GameItemResponse>> GetAsync(string name);
        public Task<List<GameItemResponse>> GetByHashNameAsync(string hash);
        public Task<List<GameItemResponse>> GetByTypeIdAsync(Guid id);
        public Task<List<GameItemResponse>> GetByRarityIdAsync(Guid id);
        public Task<List<GameItemResponse>> GetByQualityIdAsync(Guid id);
        public Task<List<GameItemQuality>> GetQualitiesAsync();
        public Task<List<GameItemRarity>> GetRaritiesAsync();
        public Task<List<GameItemType>> GetTypesAsync();
        public Task<GameItemResponse> CreateAsync(GameItemRequest request);
        public Task<GameItemResponse> UpdateAsync(GameItemRequest request);
        public Task<GameItemResponse> DeleteAsync(Guid id);
    }
}
