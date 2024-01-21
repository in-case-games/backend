﻿namespace Authentication.DAL.Entities;

public class User : BaseEntity
{
    public string? Email { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }

    public UserRestriction? Restriction { get; set; }
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}