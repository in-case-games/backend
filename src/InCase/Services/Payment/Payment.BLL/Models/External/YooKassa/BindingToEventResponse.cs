using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External.YooKassa;
public class BindingToEventResponse
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    /// <summary>
    /// <b>Имя события.</b> 
    /// </summary>
    /// <returns>Привязанное имя события. Пример: <i>payment.succeeded</i>, <i>payment.waiting_for_capture</i>, 
    /// <i>payment.cancelled</i></returns>
    /// <remarks>Подробнее: <a href="https://yookassa.ru/developers/using-api/webhooks?codeLang=bash#events">
    /// https://yookassa.ru/developers/using-api/webhooks?codeLang=bash#events</a>
    /// </remarks>
    [JsonPropertyName("event")] public string? Event { get; set; }
    /// <summary>
    /// <b>Url для уведомления.</b>
    /// </summary>
    /// <returns>Привязанный url для уведомлений.</returns>
    [JsonPropertyName("url")] public string? Url { get; set; }

}
