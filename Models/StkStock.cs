using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkStock
{
    public int StockId { get; set; }

    public int? ProductId { get; set; }

    public int? CurrentQuantity { get; set; }

    public int? ReservedQuantity { get; set; }

    public int? DeliveredQuantity { get; set; }

    public int? NetQuantity { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
