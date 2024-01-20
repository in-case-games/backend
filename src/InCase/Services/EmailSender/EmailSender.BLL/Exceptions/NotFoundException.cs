using EmailSender.BLL.Models;

namespace EmailSender.BLL.Exceptions
{
    public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);
}
