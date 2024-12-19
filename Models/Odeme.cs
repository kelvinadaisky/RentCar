using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Odeme
{
    public int IdOdeme { get; set; }

    public decimal? Tutar { get; set; }

    public DateOnly? OdemeTarihi { get; set; }

    public string? OdemeYontemi { get; set; }

    public string? OdemeDurumu { get; set; }

    public int IdFatura { get; set; }

    public virtual Fatura IdFaturaNavigation { get; set; } = null!;
}
