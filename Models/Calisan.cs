using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Calisan
{
    public string Tc { get; set; } = null!;

    public decimal? Maas { get; set; }

    public string? Pozisyon { get; set; }

    public int IdAjans { get; set; }

    public virtual Ajan IdAjansNavigation { get; set; } = null!;

    public virtual Kisi TcNavigation { get; set; } = null!;
}
