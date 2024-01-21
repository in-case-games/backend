using Review.BLL.Models;

namespace Review.BLL.Exceptions;

public class ForbiddenException(string message) : StatusCodeException(ErrorCodes.Forbidden, message);