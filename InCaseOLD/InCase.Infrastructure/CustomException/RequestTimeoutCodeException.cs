using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class RequestTimeoutCodeException : StatusCodeException
    {
        public RequestTimeoutCodeException(string message) : base(ErrorCodes.RequestTimeout, message)
        {
        }

        public RequestTimeoutCodeException(Exception inner) : base(ErrorCodes.RequestTimeout, inner)
        {
        }
    }
}
