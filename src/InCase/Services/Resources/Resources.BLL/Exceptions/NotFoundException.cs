using Resources.BLL.Models;

namespace Resources.BLL.Exceptions
{
    public class NotFoundException : StatusCodeException
    {
        public NotFoundException(string message) : base(ErrorCodes.NotFound, message) { }
    }
}
