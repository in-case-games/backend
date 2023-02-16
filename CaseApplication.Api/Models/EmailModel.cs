﻿namespace CaseApplication.Api.Models
{
    public class EmailModel
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        public string EmailToken { get; set; } = null!;
        public string UserIp { get; set; } = string.Empty;
        public string UserPlatforms { get; set; } = string.Empty;
    }
}
