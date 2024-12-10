using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class HasarDurumu
{
    public int IdHasarDurumu { get; set; }

    public string? Durumu { get; set; }

    public int? IdTeslimat { get; set; }

    public virtual Teslimat? IdTeslimatNavigation { get; set; }
}
