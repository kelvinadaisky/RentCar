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

    public int? Kilometre { get; set; }

    public string? PlakaNumarasi { get; set; }

    public bool? Durum { get; set; }

    public int IdAjans { get; set; }

    public virtual ICollection<AracDurumu> AracDurumus { get; } = new List<AracDurumu>();

    public virtual ICollection<Bakim> Bakims { get; } = new List<Bakim>();

    public virtual Ajan IdAjansNavigation { get; set; } = null!;

    public virtual ICollection<Sigortum> Sigorta { get; } = new List<Sigortum>();

    public virtual ICollection<Sozlesme> Sozlesmes { get; } = new List<Sozlesme>();

    public virtual ICollection<Sozlesme> IdSozlesmes { get; } = new List<Sozlesme>();
}
