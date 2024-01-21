using Withdraw.BLL.Models;

namespace Withdraw.BLL.Exceptions;

public class PaymentRequiredException(string message) : StatusCodeException(ErrorCodes.PaymentRequired, message);