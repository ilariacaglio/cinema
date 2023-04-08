using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Prenotazione
{
    public int Id { get; set; }

    public DateOnly DataS { get; set; }

    public TimeOnly OraS { get; set; }

    public int IdSala { get; set; }

    public string IdUtente { get; set; } = null!;

    public virtual ICollection<Comprende> Comprendes { get; } = new List<Comprende>();

    public virtual Utente IdUtenteNavigation { get; set; } = null!;

    public virtual Spettacolo Spettacolo { get; set; } = null!;
}
