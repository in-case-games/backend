using Support.BLL.Models;

namespace Support.BLL.Exceptions
{
    public class BadRequestException : StatusCodeException
    {
        public BadRequestException(string message) : base(ErrorCodes.BadRequest, message) { }
    }
}
