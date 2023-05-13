using InCase.Infrastructure.Common;

namespace InCase.Infrastructure.Exceptions
{
    public class ConflictException : StatusCodeException
    {
        public ConflictException(string message) : base(ErrorCodes.Conflict, message) {}
        public ConflictException(Exception inner) : base(ErrorCodes.Conflict, inner) {}
    }
}
