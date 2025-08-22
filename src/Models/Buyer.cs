using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Buyer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BuyerZipCode> BuyerZipCodes { get; set; } = new List<BuyerZipCode>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
