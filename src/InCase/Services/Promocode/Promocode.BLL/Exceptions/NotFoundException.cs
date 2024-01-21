using Promocode.BLL.Models;

namespace Promocode.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);