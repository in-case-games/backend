using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class PaymentRequiredCodeException : StatusCodeException
    {
        public PaymentRequiredCodeException(string message) : base(ErrorCodes.PaymentRequired, message)
        {
        }

        public PaymentRequiredCodeException(Exception inner) : base(ErrorCodes.PaymentRequired, inner)
        {
        }
    }
}
