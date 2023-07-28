namespace Payment.BLL.Models
{
    public static class GameMoneyEndpoint
    {
        public static readonly string url = "https://paygate.gamemoney.com";
        public static string Balance { get; } = url + "/statistics/balance";
        public static string InvoiceInfo { get; } = url + "/invoice/status";
    }
}
