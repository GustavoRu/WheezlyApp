using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Car
{
    public int Id { get; set; }

    public int Year { get; set; }

    public int MakeId { get; set; }

    public int ModelId { get; set; }

    public int SubModelId { get; set; }

    public int ZipCodeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Make Make { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();

    public virtual SubModel SubModel { get; set; } = null!;

    public virtual ZipCode ZipCode { get; set; } = null!;
}
