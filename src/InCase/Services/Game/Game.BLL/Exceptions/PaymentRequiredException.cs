using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class PaymentRequiredException(string message) : StatusCodeException(ErrorCodes.PaymentRequired, message);
}
