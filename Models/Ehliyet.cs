using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Ehliyet
{
    public int IdEhliyet { get; set; }

    public string? EhliyetNo { get; set; }

    public DateOnly? EhliyetTarihi { get; set; }

    public DateOnly? SonGecerlilikTarihi { get; set; }

    public string? Kategori { get; set; }

    public string MusteriTc { get; set; } = null!;

    public virtual Musteri MusteriTcNavigation { get; set; } = null!;
}
