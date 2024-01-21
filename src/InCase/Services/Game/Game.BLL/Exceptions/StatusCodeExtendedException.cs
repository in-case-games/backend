using System.Collections;

namespace Game.BLL.Exceptions;

public class StatusCodeExtendedException(int statusCode, string message, IEnumerable data) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;
    public new IEnumerable Data { get; set; } = data;
}