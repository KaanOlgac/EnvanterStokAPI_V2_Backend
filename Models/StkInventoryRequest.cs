using System;
using System.Collections.Generic;

namespace EnvanterStokAPI.Models;

public partial class StkInventoryRequest
{
    public int RequestId { get; set; }

    public int? RequestedByUserId { get; set; }

    public int? ProductId { get; set; }

    public int? RequestedQuantity { get; set; }

    public int? ApprovedQuantity { get; set; }

    public int? DeliveredQuantity { get; set; }

    public int? RemainingQuantity { get; set; }

    public char? StatusCode { get; set; }

    public DateTime? RequestDate { get; set; }

    public int? ApprovedByUserId { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? DeliveredByUserId { get; set; }

    public string? SecurityApprovalStatus { get; set; }

    public DateTime? DeliveredDate { get; set; }

    public string? Note { get; set; }
}
