using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Make
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
