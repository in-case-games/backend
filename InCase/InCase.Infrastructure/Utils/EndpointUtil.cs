﻿using InCase.Domain.Entities;
using InCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Infrastructure.Utils
{
    public class EndpointUtil
    {
        public static async Task<IActionResult> GetById<T>(
            Guid id, IDbContextFactory<ApplicationDbContext> contextFactory) 
            where T: BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return result is null ? 
                ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {id} не найдена") : 
                ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> GetAll<T>(IDbContextFactory<ApplicationDbContext> contextFactory) 
            where T : BaseEntity
        {
            await using ApplicationDbContext context = await contextFactory.CreateDbContextAsync();

            List<T> result = await context.Set<T>()
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> Create<T>(T entity, ApplicationDbContext context) 
            where T : BaseEntity
        {
            try
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(entity);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Update<T>(T entityNew, ApplicationDbContext context) 
            where T : BaseEntity
        {
            T? entityOld = await context.Set<T>()
                .FirstOrDefaultAsync(t => t.Id == entityNew.Id);

            if (entityOld is null)
                return ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {entityNew.Id} не найдена");

            try
            {
                context.Entry(entityOld).CurrentValues.SetValues(entityNew);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(entityNew);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Delete<T>(Guid id, ApplicationDbContext context) 
            where T : BaseEntity
        {
            T? result = await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (result is null)
                return ResponseUtil.NotFound("Запись таблицы " + typeof(T).Name + $" по {id} не найдена");

            context.Set<T>().Remove(result);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(result);
        }

        public static async Task<IActionResult> Update<T>(T entityOld, T entityNew, ApplicationDbContext context)
            where T : BaseEntity
        {
            try
            {
                context.Entry(entityOld).CurrentValues.SetValues(entityNew);
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(entityNew);
            }
            catch (Exception ex)
            {
                return ResponseUtil.UnknownError(ex);
            }
        }

        public static async Task<IActionResult> Delete<T>(T entity, ApplicationDbContext context)
            where T : BaseEntity
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

            return ResponseUtil.Ok(entity);
        }
    }
}
