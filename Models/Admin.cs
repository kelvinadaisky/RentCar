using System;
using System.Collections.Generic;

namespace RentCar.Models;

public partial class Admin
{
    public string Tc { get; set; } = null!;

    public string? Sifre { get; set; }

    public virtual Kisi TcNavigation { get; set; } = null!;
}
