using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;

public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);