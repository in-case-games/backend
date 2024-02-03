using System.Text.Json.Serialization;

namespace Payment.BLL.Models.External.YooKassa;
public class BindingToEventRequest
{
    private readonly string[] _validEvents = new string[]
    {
        "payment.succeeded",
        "payment.waiting_for_capture",
        "payment.cancelled"
    };

    /// <summary>
    /// <b>Имя события.</b> С помощью него можно определять, какое событие вы будете отслеживать. 
    /// </summary>
    /// <remarks>Пример: <i>payment.succeeded</i>, <i>payment.waiting_for_capture</i>, 
    /// <i>payment.cancelled</i><br/>
    /// Подробнее: <a href="https://yookassa.ru/developers/using-api/webhooks?codeLang=bash#events">
    /// https://yookassa.ru/developers/using-api/webhooks?codeLang=bash#events</a>
    /// </remarks>
    [JsonPropertyName("event")] public string? Event { get; set; }
    /// <summary>
    /// <b>Url для уведомления.</b> Используется для привязки url к уведомлениям от событий YooKassa.
    /// </summary>
    /// <remarks>Пример: <b>https://localhost:9999/payments/notify</b></remarks>
    [JsonPropertyName("url")] public string? Url { get; set; }

    public bool IsValid()
    {
        if (!_validEvents.Any(x => x.ToLower() == Event.ToLower()))
            return false;

        return true;
    }

}
