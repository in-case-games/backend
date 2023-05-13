namespace InCase.Domain.Endpoints
{
    /// <summary>
    /// class <c>PaygateEndpoint</c> enum model a paygate.gamemoney.com endpoints
    /// </summary>
    public static class PaygateEndpoint
    {
        static readonly string url = "https://paygate.gamemoney.com";
        public static string Balance { get; } = url + "/statistics/balance";
        public static string Transfer { get; } = url + "/checkout/insert";
        public static string InvoiceInfo { get; } = url + "/invoice/status";
    }
}
