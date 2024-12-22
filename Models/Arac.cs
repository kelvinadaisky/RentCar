using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Arac
{
    public int IdAraba { get; set; }

    public string? Marka { get; set; }

    public string? Model { get; set; }

    public int? UretimYili { get; set; }

    public string? Renk { get; set; }

    public string PlakaNumarasi { get; set; } = null!;

    public int? IdAjans { get; set; }

    public virtual AracDurumu? AracDurumu { get; set; }

    public virtual Bakim? Bakim { get; set; }

    public virtual Ajan? IdAjansNavigation { get; set; }

    public virtual Sigortum? Sigortum { get; set; }

    public virtual ICollection<Sozlesme> Sozlesmes { get; } = new List<Sozlesme>();
}
