﻿using Review.BLL.Models;

namespace Review.BLL.Exceptions
{
    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException(string message) : base(ErrorCodes.Forbidden, message) { }
        public ForbiddenException(Exception inner) : base(ErrorCodes.Forbidden, inner) { }
    }
}
