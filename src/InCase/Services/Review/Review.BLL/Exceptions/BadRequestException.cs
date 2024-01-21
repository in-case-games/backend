using Review.BLL.Models;

namespace Review.BLL.Exceptions;

public class BadRequestException(string message) : StatusCodeException(ErrorCodes.BadRequest, message);