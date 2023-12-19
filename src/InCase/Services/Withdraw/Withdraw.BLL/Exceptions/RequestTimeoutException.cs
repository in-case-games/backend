using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions
{
    public class RequestTimeoutException : StatusCodeException
    {
        public RequestTimeoutException(string message) : base(ErrorCodes.RequestTimeout, message) { }
    }
}
