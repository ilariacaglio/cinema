using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Comprende
{
    public int IdPosto { get; set; }

    public int IdSala { get; set; }

    public int IdPrenotazione { get; set; }

    public DateOnly DataS { get; set; }

    public TimeOnly OraS { get; set; }

    public string IdUtente { get; set; } = null!;

    public virtual Posto Id { get; set; } = null!;

    public virtual Prenotazione Prenotazione { get; set; } = null!;
}
