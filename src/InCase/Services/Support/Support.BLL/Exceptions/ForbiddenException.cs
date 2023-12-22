using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException(string message) : base(ErrorCodes.Forbidden, message) { }
    }
}
