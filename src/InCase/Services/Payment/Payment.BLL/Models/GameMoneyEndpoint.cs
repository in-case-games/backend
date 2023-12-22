namespace Payment.BLL.Models
{
    public static class GameMoneyEndpoint
    {
        public static readonly string Url = "https://paygate.gamemoney.com";
        public static string Balance { get; } = Url + "/statistics/balance";
        public static string InvoiceInfo { get; } = Url + "/invoice/status";
    }
}
