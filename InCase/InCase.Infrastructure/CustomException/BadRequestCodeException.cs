using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class BadRequestCodeException : StatusCodeException
    {
        public BadRequestCodeException(string message) : base(ErrorCodes.BadRequest, message)
        {
        }

        public BadRequestCodeException(Exception inner) : base(ErrorCodes.BadRequest, inner)
        {
        }
    }
}
