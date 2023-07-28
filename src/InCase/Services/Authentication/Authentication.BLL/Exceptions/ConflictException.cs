using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions
{
    public class ConflictException : StatusCodeException
    {
        public ConflictException(string message) : base(ErrorCodes.Conflict, message) { }
        public ConflictException(Exception inner) : base(ErrorCodes.Conflict, inner) { }
    }
}
