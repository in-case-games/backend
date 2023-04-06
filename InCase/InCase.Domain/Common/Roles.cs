namespace InCase.Domain.Common
{
    public static class Roles
    {
        public const string User = "user";
        public const string Support = "support";
        public const string Admin = "admin";
        public const string Owner = "owner";
        public const string Bot = "bot";
        public const string SupportOwnerBot = $"{Support},{Owner},{Bot}";
        public const string AdminOwnerBot = $"{Admin},{Owner},{Bot}";
        public const string All = $"{User},{Support},{Admin},{Owner},{Bot}";
        public const string AllExceptUser = $"{Support},{Admin},{Owner},{Bot}";
        public const string AllExceptAdmin = $"{User},{Support},{Owner},{Bot}";
        public const string AllExceptSupport = $"{User},{Admin},{Owner},{Bot}";
    }
}
