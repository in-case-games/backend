using Payment.BLL.Models;

namespace Payment.BLL.Exceptions;

public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);