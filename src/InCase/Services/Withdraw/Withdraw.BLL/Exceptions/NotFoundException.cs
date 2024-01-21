using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);