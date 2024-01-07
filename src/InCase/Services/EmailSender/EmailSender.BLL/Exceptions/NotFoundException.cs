using EmailSender.BLL.Models;

namespace EmailSender.BLL.Exceptions
{
    public class NotFoundException : StatusCodeException
    {
        public NotFoundException(string message) : base(ErrorCodes.NotFound, message) { }
    }
}
