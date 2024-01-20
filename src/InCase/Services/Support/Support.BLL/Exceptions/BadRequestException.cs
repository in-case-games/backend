using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);
}
