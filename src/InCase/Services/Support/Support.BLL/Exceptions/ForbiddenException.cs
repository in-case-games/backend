using Support.BLL.Models;

namespace Support.BLL.Exceptions;

public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);