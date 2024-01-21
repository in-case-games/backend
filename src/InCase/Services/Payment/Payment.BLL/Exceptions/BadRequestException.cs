using Payment.BLL.Models;

namespace Payment.BLL.Exceptions;

public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);