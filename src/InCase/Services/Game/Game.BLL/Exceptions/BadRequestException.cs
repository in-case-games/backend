using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);
}
