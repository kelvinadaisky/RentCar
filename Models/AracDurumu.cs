using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class AracDurumu
{
    public int IdDurum { get; set; }

    public string Aciklama { get; set; } = null!;

    public DateOnly? GuncellemeTarihi { get; set; }

    public int? IdAraba { get; set; }

    public virtual Arac? IdArabaNavigation { get; set; }
}
