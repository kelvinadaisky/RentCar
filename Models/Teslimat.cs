using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Teslimat
{
    public int IdTeslimat { get; set; }

    public DateOnly? TeslimatTarihi { get; set; }

    public DateOnly? GeriDonusTarihi { get; set; }

    public string? Durum { get; set; }

    public int IdSozlesme { get; set; }

    public virtual HasarDurumu? HasarDurumu { get; set; }

    public virtual Sozlesme IdSozlesmeNavigation { get; set; } = null!;
}
