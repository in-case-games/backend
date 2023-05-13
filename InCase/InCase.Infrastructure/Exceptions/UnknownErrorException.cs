using InCase.Infrastructure.Common;

namespace InCase.Infrastructure.Exceptions
{
    public class UnknownErrorException : StatusCodeException
    {
        public UnknownErrorException(string message) : base(ErrorCodes.UnknownError, message) { }
        public UnknownErrorException(Exception inner) : base(ErrorCodes.UnknownError, inner) { }
    }
}
