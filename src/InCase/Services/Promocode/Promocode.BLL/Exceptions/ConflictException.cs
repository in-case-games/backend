using Promocode.BLL.Models;

namespace Promocode.BLL.Exceptions;

public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);