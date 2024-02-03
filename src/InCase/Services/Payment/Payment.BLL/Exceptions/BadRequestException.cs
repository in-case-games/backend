using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;

public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);