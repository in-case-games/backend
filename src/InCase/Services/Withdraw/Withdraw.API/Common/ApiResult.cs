namespace Withdraw.API.Common
{
    public class ApiResult<T>(int code, T data)
    {
        public int Code { get; set; } = code;
        public T Data { get; set; } = data;

        public static ApiResult<T> Ok(T data) => new(0, data);
    }
}
