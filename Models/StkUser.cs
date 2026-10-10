using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkUser
{
    public int UserId { get; set; }

    public int? RoleId { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public string? FullName { get; set; }

    public string? EmailAddress { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
