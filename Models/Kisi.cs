using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Kisi
{
    public string Tc { get; set; } = null!;

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? EPosta { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Calisan? Calisan { get; set; }

    public virtual Musteri? Musteri { get; set; }
}
