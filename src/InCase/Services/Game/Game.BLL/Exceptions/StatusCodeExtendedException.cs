using System.Collections;

namespace Game.BLL.Exceptions
{
    public class StatusCodeExtendedException : Exception
    {
        public int StatusCode { get; set; }
        public new IEnumerable Data { get; set; }

        public StatusCodeExtendedException(int statusCode, string message, IEnumerable data) : base(message)
        {
            StatusCode = statusCode;
            Data = data;
        }

        public StatusCodeExtendedException(int statusCode, Exception inner, IEnumerable data)
            : this(statusCode, inner.ToString(), data) { }
    }
}
