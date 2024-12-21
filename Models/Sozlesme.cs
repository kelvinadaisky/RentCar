using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Sozlesme
{
    public int IdSozlesme { get; set; }

    public string? IdMusteri { get; set; }

    public int? IdAraba { get; set; }

    public decimal ToplamTutar { get; set; }

    public string KiraSekli { get; set; } = null!;

    public DateOnly? CikisTarihi { get; set; }

    public DateOnly? DonusTarihi { get; set; }

    public string? Durum { get; set; }

    public virtual Fatura? Fatura { get; set; }

    public virtual Arac? IdArabaNavigation { get; set; }

    public virtual Musteri? IdMusteriNavigation { get; set; }

    public virtual Teslimat? Teslimat { get; set; }
}
