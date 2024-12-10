using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Sigortum
{
    public int IdSigorta { get; set; }

    public string? SigortaTuru { get; set; }

    public string? SigortaFirma { get; set; }

    public DateOnly? SonTarih { get; set; }

    public int? IdAraba { get; set; }

    public virtual Arac? IdArabaNavigation { get; set; }
}
