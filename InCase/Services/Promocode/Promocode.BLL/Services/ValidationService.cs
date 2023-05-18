using Promocode.BLL.Exceptions;
using Promocode.BLL.Models;

namespace Promocode.BLL.Services
{
    public class ValidationService
    {
        public static void IsPromocode(PromocodeRequest request)
        {
            if (request.Discount >= 1M || request.Discount <= 0)
                throw new BadRequestException("Скидка промокода должна быть больше 0 и меньше 1");
            if (request.NumberActivations <= 0)
                throw new BadRequestException("Количество активаций должно быть больше 0");
        }
    }
}
