using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);
}
