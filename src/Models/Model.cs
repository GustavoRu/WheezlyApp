using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Model
{
    public int Id { get; set; }

    public int MakeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Make Make { get; set; } = null!;

    public virtual ICollection<SubModel> SubModels { get; set; } = new List<SubModel>();
}
