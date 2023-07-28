using Identity.BLL.Models;

namespace Identity.BLL.Exceptions
{
    public class BadRequestException : StatusCodeException
    {
        public BadRequestException(string message) : base(ErrorCodes.BadRequest, message) { }
        public BadRequestException(Exception inner) : base(ErrorCodes.BadRequest, inner) { }
    }
}
