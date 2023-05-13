using InCase.Infrastructure.Common;

namespace InCase.Infrastructure.Exceptions
{
    public class NotFoundException : StatusCodeException
    {
        public NotFoundException(string message) : base(ErrorCodes.NotFound, message) {}
        public NotFoundException(Exception inner) : base(ErrorCodes.NotFound, inner) {}
    }
}
