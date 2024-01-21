using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions;

public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);