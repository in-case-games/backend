using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;

public class RequestTimeoutException(string message) : StatusCodeException(ErrorCodes.RequestTimeout, message);