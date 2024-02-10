namespace Authentication.API.Common;
public class ApiResult<T>(int code, T data)
{
    public int Code { get; set; } = code;
    public T Data { get; set; } = data;

    public static ApiResult<T> Ok(T data) => new(0, data);

    public static ApiResult<string> SentEmail(string message = "Сообщение отправлено на email почту") =>
        new(2, message);
}
