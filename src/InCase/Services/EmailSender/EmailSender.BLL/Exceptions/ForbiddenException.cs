using EmailSender.BLL.Models;

namespace EmailSender.BLL.Exceptions;
public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);