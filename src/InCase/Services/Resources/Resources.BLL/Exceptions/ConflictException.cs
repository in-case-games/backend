using Resources.BLL.Models;

namespace Resources.BLL.Exceptions;

public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);