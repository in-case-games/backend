using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);
}
