namespace Payment.BLL.Models.External.YooKassa;
public static class PaymentEventTypes
{
    public static readonly string Waiting = "payment.waiting_for_capture";
    public static readonly string Succeeded = "payment.succeeded";
    public static readonly string Cancelled = "payment.cancelled";
}
