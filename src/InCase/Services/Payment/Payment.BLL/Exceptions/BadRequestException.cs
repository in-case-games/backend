using Payment.BLL.Models;

namespace Payment.BLL.Exceptions
{
    public class BadRequestException : StatusCodeException
    {
        public BadRequestException(string message) : base(ErrorCodes.BadRequest, message) { }
    }
}
