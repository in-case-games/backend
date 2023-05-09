using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class NotFoundCodeException : StatusCodeException
    {
        public NotFoundCodeException(string message) : base(ErrorCodes.NotFound, message)
        {
        }

        public NotFoundCodeException(Exception inner) : base(ErrorCodes.NotFound, inner)
        {
        }
    }
}
