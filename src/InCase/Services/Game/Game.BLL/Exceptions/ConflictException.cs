using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);
}
