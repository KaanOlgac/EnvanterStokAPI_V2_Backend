using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkStockMovement
{
    public int MovementId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public char? MovementType { get; set; }

    public int? Quantity { get; set; }

    public DateTime? MovementDate { get; set; }

    public string? Note { get; set; }

    public int? RequestId { get; set; }
}
