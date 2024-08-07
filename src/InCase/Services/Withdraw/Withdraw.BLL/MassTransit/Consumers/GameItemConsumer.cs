﻿using Infrastructure.MassTransit.Resources;
using MassTransit;
using Withdraw.BLL.Interfaces;

namespace Withdraw.BLL.MassTransit.Consumers;
public class GameItemConsumer(IGameItemService itemService) : IConsumer<GameItemTemplate>
{
	public async Task Consume(ConsumeContext<GameItemTemplate> context)
	{
		var item = await itemService.GetAsync(context.Message.Id);

		if (item is not null && context.Message.IsDeleted) await itemService.DeleteAsync(context.Message.Id);
		else if (!context.Message.IsDeleted) {
			if (item is null) await itemService.CreateAsync(context.Message);
			else await itemService.UpdateAsync(context.Message);
		}
	}
}