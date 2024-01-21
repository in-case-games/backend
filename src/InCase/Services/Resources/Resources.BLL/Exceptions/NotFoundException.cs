using Resources.BLL.Models;

namespace Resources.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);