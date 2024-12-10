using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Sozlesme
{
    public int IdSozlesme { get; set; }

    public DateOnly? ImzalanmaTarihi { get; set; }

    public int? Sure { get; set; }

    public string? Kosullar { get; set; }

    public string? IdMusteri { get; set; }

    public int? IdAraba { get; set; }

    public virtual ICollection<Fatura> Faturas { get; } = new List<Fatura>();

    public virtual Arac? IdArabaNavigation { get; set; }

    public virtual Musteri? IdMusteriNavigation { get; set; }

    public virtual ICollection<Teslimat> Teslimats { get; } = new List<Teslimat>();

    public virtual ICollection<Arac> IdArabas { get; } = new List<Arac>();
}
