using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class BadRequestException : StatusCodeException
    {
        public BadRequestException(string message) : base(ErrorCodes.BadRequest, message) { }
    }
}
