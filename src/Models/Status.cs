using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool RequiresDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
