using Payment.BLL.Models.Internal;

namespace Payment.BLL.Exceptions;
public class UnknownException(string message) : StatusCodeException(ErrorCodes.UnknownError, message);