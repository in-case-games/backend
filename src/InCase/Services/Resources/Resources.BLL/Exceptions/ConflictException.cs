using Resources.BLL.Models;

namespace Resources.BLL.Exceptions
{
    public class ConflictException : StatusCodeException
    {
        public ConflictException(string message) : base(ErrorCodes.Conflict, message) { }
    }
}
