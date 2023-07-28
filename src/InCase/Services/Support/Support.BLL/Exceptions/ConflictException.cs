using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class ConflictException : StatusCodeException
    {
        public ConflictException(string message) : base(ErrorCodes.Conflict, message) { }
        public ConflictException(Exception inner) : base(ErrorCodes.Conflict, inner) { }
    }
}
