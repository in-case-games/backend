using Review.BLL.Models;

namespace Review.BLL.Exceptions;

public class NotFoundException(string message) : StatusCodeException(ErrorCodes.NotFound, message);