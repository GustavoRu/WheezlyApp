using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class ZipCode
{
    public int Id { get; set; }

    public string ZipCode1 { get; set; } = null!;

    public virtual ICollection<BuyerZipCode> BuyerZipCodes { get; set; } = new List<BuyerZipCode>();

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
