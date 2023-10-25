using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class UnauthorizedException : StatusCodeException
    {
        public UnauthorizedException(string message) : base(ErrorCodes.Unauthorized, message) { }
        public UnauthorizedException(Exception inner) : base(ErrorCodes.Unauthorized, inner) { }
    }
}
