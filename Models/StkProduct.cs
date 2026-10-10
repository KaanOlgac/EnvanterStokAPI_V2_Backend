using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkProduct
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? UnitName { get; set; }

    public int? MinStockLevel { get; set; }

    public int? ClearanceLevel { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
