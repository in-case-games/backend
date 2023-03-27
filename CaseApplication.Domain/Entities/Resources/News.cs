﻿namespace CaseApplication.Domain.Entities.Resources
{
    public class News : BaseEntity
    {
        public string? NewsName { get; set; }
        public DateTime? NewsDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsImage { get; set; }
    }
}