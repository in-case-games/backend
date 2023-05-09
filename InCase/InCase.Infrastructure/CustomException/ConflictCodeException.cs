using InCase.Domain.Common;

namespace InCase.Infrastructure.CustomException
{
    public class ConflictCodeException : StatusCodeException
    {
        public ConflictCodeException(string message) : base(ErrorCodes.Conflict, message)
        {
        }

        public ConflictCodeException(Exception inner) : base(ErrorCodes.Conflict, inner)
        {
        }
    }
}
