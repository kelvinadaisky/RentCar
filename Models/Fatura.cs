using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Fatura
{
    public int IdFatura { get; set; }

    public decimal? OdenenTutar { get; set; }

    public DateOnly? DuzenlenmeTarihi { get; set; }

    public DateOnly? VadeTarihi { get; set; }

    public int IdSozlesme { get; set; }

    public virtual Sozlesme IdSozlesmeNavigation { get; set; } = null!;

    public virtual Odeme? Odeme { get; set; }
}
