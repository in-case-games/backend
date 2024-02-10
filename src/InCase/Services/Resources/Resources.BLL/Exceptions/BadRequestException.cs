using Resources.BLL.Models;

namespace Resources.BLL.Exceptions;
public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);