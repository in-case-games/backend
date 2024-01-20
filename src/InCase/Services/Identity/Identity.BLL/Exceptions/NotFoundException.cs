using Identity.BLL.Models;

namespace Identity.BLL.Exceptions
{
    public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);
}
