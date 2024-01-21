using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions;

public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);