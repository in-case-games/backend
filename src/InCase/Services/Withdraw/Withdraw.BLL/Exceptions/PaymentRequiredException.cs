using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions
{
    public class PaymentRequiredException : StatusCodeException
    {
        public PaymentRequiredException(string message) : base(ErrorCodes.PaymentRequired, message) { }
    }
}
