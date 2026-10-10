using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? Description { get; set; }

    public bool? IsAvtive { get; set; }

    public int? ClearanceLevel { get; set; }
}
