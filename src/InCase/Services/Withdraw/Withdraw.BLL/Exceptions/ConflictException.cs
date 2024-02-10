using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions;
public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);