using InCase.Domain.Entities;
using InCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Infrastructure.Utils
{
    public class EndpointUtil
    {
        public static async Task<IActionResult> GetById<T>(Guid id, IDbContextFactory<ApplicationDbContext> contextFactory) where T: BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result is null)
                return ResponseUtil.NotFound(typeof(T).Name);

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> GetAll<T>(IDbContextFactory<ApplicationDbContext> contextFactory) where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            List<T> result = await context.Set<T>()
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> Create<T>(T entity, IDbContextFactory<ApplicationDbContext> contextFactory) where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            try
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(entity);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public static async Task<IActionResult> Update<T>(T entity, IDbContextFactory<ApplicationDbContext> contextFactory) where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (result is null)
                return ResponseUtil.NotFound(typeof(T).Name);

            try
            {
                context.Entry(result).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        public static async Task<IActionResult> Delete<T>(Guid id, IDbContextFactory<ApplicationDbContext> contextFactory) where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result is null)
                return ResponseUtil.NotFound(typeof(T).Name);

            context.Set<T>().Remove(result);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(typeof(T).Name);
        }
    }
}
