using Identity.BLL.Models;

namespace Identity.BLL.Exceptions
{
    public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);
}
