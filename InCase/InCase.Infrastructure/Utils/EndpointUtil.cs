using InCase.Domain.Entities;
using InCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace InCase.Infrastructure.Utils
{
    public class EndpointUtil
    {
        public static async Task<IActionResult> GetById<T>(Guid id, IDbContextFactory<ApplicationDbContext> contextFactory, CancellationToken cancellationToken = default) 
            where T: BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync(cancellationToken);

            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            return result is null ? 
                ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {id} не найдена") : 
                ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> GetAll<T>(IDbContextFactory<ApplicationDbContext> contextFactory, CancellationToken cancellationToken = default) 
            where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync(cancellationToken);

            List<T> result = await context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> Create<T>(T entity, ApplicationDbContext context, CancellationToken cancellationToken = default) 
            where T : BaseEntity
        {
            try
            {
                await context.Set<T>().AddAsync(entity, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return ResponseUtil.Ok(entity);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Update<T>(T entityNew, ApplicationDbContext context, CancellationToken cancellationToken = default) 
            where T : BaseEntity
        {
            T? entityOld = await context.Set<T>()
                .FirstOrDefaultAsync(f => f.Id == entityNew.Id, cancellationToken);

            if (entityOld is null)
                return ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {entityNew.Id} не найдена");

            try
            {
                context.Entry(entityOld).CurrentValues.SetValues(entityNew);
                await context.SaveChangesAsync(cancellationToken);

                return ResponseUtil.Ok(entityNew);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Delete<T>(Guid id, ApplicationDbContext context, CancellationToken cancellationToken = default) 
            where T : BaseEntity
        {
            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (result is null)
                return ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {id} не найдена");

            context.Set<T>().Remove(result);
            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> Update<T>(T entityOld, T entityNew, ApplicationDbContext context, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            try
            {
                context.Entry(entityOld).CurrentValues.SetValues(entityNew);
                await context.SaveChangesAsync(cancellationToken);

                return ResponseUtil.Ok(entityNew);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Delete<T>(T entity, ApplicationDbContext context, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.Ok(entity);
        }
    }
}
