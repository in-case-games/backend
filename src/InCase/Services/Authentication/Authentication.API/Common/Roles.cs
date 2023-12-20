namespace Authentication.API.Common
{
    public class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string Owner = "owner";
        public const string Bot = "bot";
        public const string All = $"{User},{Admin},{Owner},{Bot}";
    }
}
