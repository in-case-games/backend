using Review.BLL.Models;

namespace Review.BLL.Exceptions;
public class ConflictException(string message) : StatusCodeException(ErrorCodes.Conflict, message);