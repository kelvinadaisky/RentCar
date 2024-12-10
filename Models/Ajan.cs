using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Ajan
{
    public int IdAjans { get; set; }

    public string? AjansAdi { get; set; }

    public string? Adres { get; set; }

    public string? Telefon { get; set; }

    public string? EPosta { get; set; }

    public string? AdminTc { get; set; }

    public virtual Admin? AdminTcNavigation { get; set; }

    public virtual ICollection<Arac> Aracs { get; } = new List<Arac>();

    public virtual ICollection<Calisan> Calisans { get; } = new List<Calisan>();
}
