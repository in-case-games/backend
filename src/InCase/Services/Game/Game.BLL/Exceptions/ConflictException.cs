using Game.BLL.Models;

namespace Game.BLL.Exceptions
{
    public class ConflictException : StatusCodeException
    {
        public ConflictException(string message) : base(ErrorCodes.Conflict, message) { }
    }
}
