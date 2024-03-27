﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.DAL.Entities;
public class UserRole : BaseEntity
{
    [MaxLength(15)]
    public string? Name { get; set; }

    [JsonIgnore]
    public UserAdditionalInfo? AdditionalInfo { get; set; }
}