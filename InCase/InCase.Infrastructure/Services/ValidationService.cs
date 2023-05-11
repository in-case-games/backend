using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Infrastructure.Services
{
    public class ValidationService
    {
        public async static Task CheckOwnerSupportTopic(Guid id, Guid userId, ApplicationDbContext context)
        {
            SupportTopic topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(sp => sp.Id == id) ??
                throw new NotFoundCodeException("Топик не найден");

            if (topic.UserId != userId)
                throw new ForbiddenCodeException("Только создатель топика может его просматривать");
        }

        public static void CheckBadRequestPromocode(PromocodeDto promocodeDto)
        {
            if (promocodeDto.Discount >= 1M || promocodeDto.Discount <= 0)
                throw new BadRequestCodeException("Скидка промокода должна быть больше 0 и меньше 1");
            if (promocodeDto.NumberActivations <= 0)
                throw new BadRequestCodeException("Количество активаций должно быть больше 0");
        }

        public async static Task CheckNotFoundGameItem(GameItemDto itemDto, ApplicationDbContext context)
        {
            if (!await context.Games.AnyAsync(a => a.Id == itemDto.GameId))
                throw new NotFoundCodeException("Игра не найден");
            if (!await context.GameItemTypes.AnyAsync(a => a.Id == itemDto.TypeId))
                throw new NotFoundCodeException("Тип предмета не найден");
            if (!await context.GameItemRarities.AnyAsync(a => a.Id == itemDto.RarityId))
                throw new NotFoundCodeException("Редкость предмета не найдена");
            if (!await context.GameItemQualities.AnyAsync(a => a.Id == itemDto.QualityId))
                throw new NotFoundCodeException("Качество предмета не найдено");
        }

        public static bool IsActiveBanner(in UserPathBanner path, in LootBox box) => 
            path is not null && box.Banner!.IsActive;

        public static bool IsValidUserPassword(in User user, string password)
        {
            string hash = EncryptorService.GenerationHashSHA512(password, Convert
                .FromBase64String(user.PasswordSalt!));

            return hash == user.PasswordHash;
        }

        public static bool IsValidToken(in User user, ClaimsPrincipal principal, string type)
        {
            string? lifetime = principal?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            DateTimeOffset lifetimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(lifetime ?? "0"));
            DateTime lifetimeDateTime = lifetimeOffset.UtcDateTime;

            string? hash = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;
            string? email = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? tokenType = principal?.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;

            return (DateTime.UtcNow < lifetimeDateTime &&
                user.PasswordHash == hash &&
                user.Email == email &&
                tokenType == type);
        }
    }
}
