using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;
public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);