using Promocode.BLL.Models;

namespace Promocode.BLL.Exceptions
{
    public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);
}
