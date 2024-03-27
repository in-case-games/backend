using Resources.BLL.Exceptions;
using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Services;
public static class ValidationService
{
    public static void IsGameItem(GameItemRequest request)
    {
        if (request.Cost <= 0) 
            throw new BadRequestException("Предмет должен стоить больше 0");
        if (request.Name is null || request.Name.Length < 3 || request.Name.Length > 50) 
            throw new BadRequestException("Длина названия должна находиться между 3 и 50");
    }

    public static void IsGroupLootBox(GroupLootBox request)
    {
        if (request.Name is null || request.Name.Length < 3 || request.Name.Length > 50)
            throw new BadRequestException("Длина названия должна находиться между 3 и 50");
    }

    public static void IsLootBox(LootBoxRequest request)
    {
        if (request.Cost <= 0)
            throw new BadRequestException("Кейс должен стоить больше 0");
        if (request.Name is null || request.Name.Length < 3 || request.Name.Length > 50)
            throw new BadRequestException("Длина названия должна находиться между 3 и 50");
    }
}