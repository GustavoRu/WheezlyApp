using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class SubModel
{
    public int Id { get; set; }

    public int ModelId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Model Model { get; set; } = null!;
}
