namespace Payment.BLL.Exceptions;

public class StatusCodeException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;
}