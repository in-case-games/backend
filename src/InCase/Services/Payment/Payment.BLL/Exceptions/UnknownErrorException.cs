﻿using Payment.BLL.Models;

namespace Payment.BLL.Exceptions
{
    public class UnknownErrorException : StatusCodeException
    {
        public UnknownErrorException(string message) : base(ErrorCodes.UnknownError, message) { }
        public UnknownErrorException(Exception inner) : base(ErrorCodes.UnknownError, inner) { }
    }
}
