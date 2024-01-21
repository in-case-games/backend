using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);