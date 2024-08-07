﻿using Game.BLL.Interfaces;
using Infrastructure.MassTransit.Resources;
using MassTransit;

namespace Game.BLL.MassTransit.Consumers;
public class LootBoxInventoryConsumer(ILootBoxInventoryService inventoryService) : IConsumer<LootBoxInventoryTemplate>
{
    public async Task Consume(ConsumeContext<LootBoxInventoryTemplate> context)
    {
        var inventory = await inventoryService.GetAsync(context.Message.Id);

        if (inventory is not null && context.Message.IsDeleted) await inventoryService.DeleteAsync(context.Message.Id);
        else if(!context.Message.IsDeleted) {
            if (inventory is null) await inventoryService.CreateAsync(context.Message);
            else await inventoryService.UpdateAsync(context.Message);
        } 
    }
}