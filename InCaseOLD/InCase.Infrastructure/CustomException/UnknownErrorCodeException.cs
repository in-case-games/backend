using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class UnknownErrorCodeException : StatusCodeException
    {
        public UnknownErrorCodeException(string message) : base(ErrorCodes.UnknownError, message)
        {
        }

        public UnknownErrorCodeException(Exception inner) : base(ErrorCodes.UnknownError, inner)
        {
        }
    }
}
