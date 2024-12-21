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

    public virtual ICollection<Fatura> Faturas { get; } = new List<Fatura>();

    public virtual Arac? IdArabaNavigation { get; set; }

    public virtual Musteri? IdMusteriNavigation { get; set; }

    public virtual ICollection<Teslimat> Teslimats { get; } = new List<Teslimat>();

    public virtual ICollection<Arac> IdArabas { get; } = new List<Arac>();
}
