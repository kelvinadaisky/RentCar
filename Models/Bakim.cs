using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Bakim
{
    public int IdBakim { get; set; }

    public string? BakimTuru { get; set; }

    public string? Aciklama { get; set; }

    public DateOnly? BaslangicTarihi { get; set; }

    public DateOnly? BitisTarihi { get; set; }

    public decimal? Maliyet { get; set; }

    public int IdAraba { get; set; }

    public virtual Arac IdArabaNavigation { get; set; } = null!;
}
