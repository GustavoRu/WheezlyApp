using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class StatusHistory
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int StatusId { get; set; }

    public int ChangedByUserId { get; set; }

    public DateTime? StatusDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User ChangedByUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
