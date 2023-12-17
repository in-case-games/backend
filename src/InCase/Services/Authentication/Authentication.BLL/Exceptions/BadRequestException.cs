using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions
{
    public class BadRequestException : StatusCodeException
    {
        public BadRequestException(string message) : base(ErrorCodes.BadRequest, message) { }
    }
}
