using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class PaymentRequiredException : StatusCodeException
    {
        public PaymentRequiredException(string message) : base(ErrorCodes.PaymentRequired, message) { }
    }
}
