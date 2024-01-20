using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions
{
    public class UnauthorizedException(string message) : StatusCodeException(ErrorCodes.Unauthorized, message);
}
