using Promocode.BLL.Models;

namespace Promocode.BLL.Exceptions;
public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);