using Payment.BLL.Models;

namespace Payment.BLL.Exceptions;

public class RequestTimeoutException(string message) : StatusCodeException(ErrorCodes.RequestTimeout, message);