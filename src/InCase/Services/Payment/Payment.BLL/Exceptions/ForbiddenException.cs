using Payment.BLL.Models;

namespace Payment.BLL.Exceptions;

public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);