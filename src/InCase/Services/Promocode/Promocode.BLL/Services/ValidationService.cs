using Promocode.BLL.Exceptions;
using Promocode.BLL.Models;

namespace Promocode.BLL.Services;
public class ValidationService
{
    public static void IsPromoCode(PromoCodeRequest request)
    {
        if (request.Discount is >= 1M or <= 0)
            throw new BadRequestException("Скидка промокода должна быть больше 0 и меньше 1");
        if (request.NumberActivations <= 0)
            throw new BadRequestException("Количество активаций должно быть больше 0");
        if (request.Name is null || request.Name.Length < 3 || request.Name.Length > 20)
            throw new BadRequestException("Длина промокода должна быть между 3 и 20");
    }
}