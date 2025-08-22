using System;
using System.Collections.Generic;

namespace WheezlyApp.Models;

public partial class Quote
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int BuyerId { get; set; }

    public decimal CurrentAmount { get; set; }

    public bool IsCurrentQuote { get; set; }

    public int ZipCodeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Buyer Buyer { get; set; } = null!;

    public virtual Car Car { get; set; } = null!;

    public virtual ZipCode ZipCode { get; set; } = null!;
}
