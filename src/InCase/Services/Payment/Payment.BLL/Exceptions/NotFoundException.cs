using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;
public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);