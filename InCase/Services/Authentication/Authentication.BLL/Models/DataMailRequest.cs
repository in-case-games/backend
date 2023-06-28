﻿namespace Authentication.BLL.Models
{
    public class DataMailRequest
    {
        public string UserLogin { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string EmailToken { get; set; } = null!;
        public string UserIp { get; set; } = string.Empty;
        public string UserPlatforms { get; set; } = string.Empty;
    }
}
