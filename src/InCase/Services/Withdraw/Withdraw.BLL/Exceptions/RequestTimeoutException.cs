using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions;

public class RequestTimeoutException(string message) : StatusCodeException(ErrorCodes.RequestTimeout, message);