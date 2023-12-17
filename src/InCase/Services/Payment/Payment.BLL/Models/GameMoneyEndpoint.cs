namespace Payment.BLL.Models
{
    public static class GameMoneyEndpoint
    {
        public static readonly string URL = "https://paygate.gamemoney.com";
        public static string Balance { get; } = URL + "/statistics/balance";
        public static string InvoiceInfo { get; } = URL + "/invoice/status";
    }
}
