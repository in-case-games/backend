using Payment.BLL.Models;

namespace Payment.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);