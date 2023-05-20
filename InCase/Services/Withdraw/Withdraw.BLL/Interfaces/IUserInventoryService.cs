﻿using Withdraw.BLL.Models;

namespace Withdraw.BLL.Interfaces
{
    public interface IUserInventoryService
    {
        public Task<UserInventoryResponse> GetByIdAsync(Guid id);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId);
        public Task<List<UserInventoryResponse>> GetAsync(Guid userId, int count);
        public Task<decimal> SellAsync(Guid id, Guid userId);
        public Task<decimal> SellLastAsync(Guid itemId, Guid userId);
        public Task<UserInventoryResponse> ExchangeAsync(Guid id, Guid itemId, Guid userId);
    }
}