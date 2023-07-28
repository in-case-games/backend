using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions
{
    public class PaymentRequiredException : StatusCodeException
    {
        public PaymentRequiredException(string message) : base(ErrorCodes.PaymentRequired, message) { }
        public PaymentRequiredException(Exception inner) : base(ErrorCodes.PaymentRequired, inner) { }
    }
}
