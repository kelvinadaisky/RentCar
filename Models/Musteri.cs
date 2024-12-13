using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Musteri
{
    public string Tc { get; set; } = null!;

    public DateOnly? DogumTarihi { get; set; }

    public virtual Ehliyet? Ehliyet { get; set; }

    public virtual ICollection<Sozlesme> Sozlesmes { get; } = new List<Sozlesme>();

    public virtual Kisi TcNavigation { get; set; } = null!;
}
