using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}
