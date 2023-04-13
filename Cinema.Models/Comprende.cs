using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Comprende
{
    public int IdPosto { get; set; }

    public int IdPrenotazione { get; set; }

    public virtual Posto Id { get; set; } = null!;

    public virtual Prenotazione Prenotazione { get; set; } = null!;
}
