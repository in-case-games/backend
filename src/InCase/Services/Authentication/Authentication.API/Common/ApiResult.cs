using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Common
{
    public class ApiResult<T>
    {
        private const string SENT_EMAIL = "Сообщение отправлено на email почту";

        public int Code { get; set; }
        public T Data { get; set; }

        public ApiResult(int code, T data)
        {
            Code = code;
            Data = data;
        }

        public static ApiResult<T> OK(T data) =>
            new(0, data);

        public static ApiResult<string> SentEmail(string message = SENT_EMAIL) =>
            new(2, message);
    }
}
