using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class UnauthorizedCodeException : StatusCodeException
    {
        public UnauthorizedCodeException(string message) : base(ErrorCodes.Unauthorized, message)
        {
        }

        public UnauthorizedCodeException(Exception inner) : base(ErrorCodes.Unauthorized, inner)
        {
        }
    }
}
