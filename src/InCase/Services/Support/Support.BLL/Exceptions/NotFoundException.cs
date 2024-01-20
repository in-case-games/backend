using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);
}
