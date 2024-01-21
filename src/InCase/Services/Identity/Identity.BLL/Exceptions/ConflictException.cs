using Identity.BLL.Models;

namespace Identity.BLL.Exceptions;

public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);