﻿using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task CreateAsync(GameItemTemplate template);
        public Task UpdateAsync(GameItemTemplate template);
        public Task DeleteAsync(Guid id);
    }
}