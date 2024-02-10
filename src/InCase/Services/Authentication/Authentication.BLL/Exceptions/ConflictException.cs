using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions;
public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);