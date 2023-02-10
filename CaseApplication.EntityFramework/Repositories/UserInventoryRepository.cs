using AutoMapper;
using CaseApplication.DomainLayer.Dtos;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CaseApplication.EntityFramework.Repositories
{
    public class UserInventoryRepository : IUserInventoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly MapperConfiguration _mapperConfiguration = new(configuration =>
        {
            configuration.CreateMap<UserInventoryDto, UserInventory>();
        });
        public UserInventoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserInventory?> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();
            
            UserInventory? inventory = await context.UserInventory.FirstOrDefaultAsync(x => x.Id == id);
            
            if (inventory != null)
            {
                inventory.GameItem = await context.GameItem.FirstOrDefaultAsync(
                    x => x.Id == inventory.GameItemId);
            }

            return inventory;
        }

        public async Task<List<UserInventory>> GetAll(Guid userId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<UserInventory> inventories = await context.UserInventory
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach(UserInventory inventory in inventories)
            {
                inventory.GameItem = await context.GameItem.FirstOrDefaultAsync(
                    x => x.Id == inventory.GameItemId);
            }

            return inventories;
        }

        public async Task<bool> Create(UserInventoryDto userInventoryDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? newInventory = mapper.Map<UserInventory>(userInventoryDto);

            await context.UserInventory.AddAsync(newInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(UserInventoryDto userInventoryDto)
        {
            IMapper? mapper = _mapperConfiguration.CreateMapper();

            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? oldInventory = await context.UserInventory
                .FirstOrDefaultAsync(x => x.Id == userInventoryDto.Id);

            if(oldInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            UserInventory? newInventory = mapper.Map<UserInventory>(userInventoryDto);

            context.Entry(oldInventory).CurrentValues.SetValues(newInventory);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            UserInventory? searchUserInventory = await context.UserInventory
                .FirstOrDefaultAsync(x => x.Id == id);

            if (searchUserInventory is null) throw new Exception("There is no such user inventory, " +
                "review what data comes from the api");

            context.UserInventory.Remove(searchUserInventory);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
