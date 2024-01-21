using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions;

public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);