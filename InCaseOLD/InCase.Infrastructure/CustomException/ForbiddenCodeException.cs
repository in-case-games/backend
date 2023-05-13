using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class ForbiddenCodeException : StatusCodeException
    {
        public ForbiddenCodeException(string message) : base(ErrorCodes.Forbidden, message)
        {
        }

        public ForbiddenCodeException(Exception inner) : base(ErrorCodes.Forbidden, inner)
        {
        }
    }
}
