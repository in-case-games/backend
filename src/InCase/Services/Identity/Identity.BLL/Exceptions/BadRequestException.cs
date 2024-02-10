using Identity.BLL.Models;

namespace Identity.BLL.Exceptions;
public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);