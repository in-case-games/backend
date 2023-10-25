﻿using Authentication.BLL.Models;

namespace Authentication.BLL.Exceptions
{
    public class RequestTimeoutException : StatusCodeException
    {
        public RequestTimeoutException(string message) : base(ErrorCodes.RequestTimeout, message) { }
        public RequestTimeoutException(Exception inner) : base(ErrorCodes.RequestTimeout, inner) { }
    }
}
