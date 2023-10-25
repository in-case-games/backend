namespace Promocode.BLL.Exceptions
{
    public class StatusCodeException : Exception
    {
        public int StatusCode { get; set; }

        public StatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public StatusCodeException(int statusCode, Exception inner)
            : this(statusCode, inner.ToString()) { }
    }
}
