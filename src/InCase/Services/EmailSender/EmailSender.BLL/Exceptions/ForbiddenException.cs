using EmailSender.BLL.Models;

namespace EmailSender.BLL.Exceptions
{
    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException(string message) : base(ErrorCodes.Forbidden, message) { }
    }
}
