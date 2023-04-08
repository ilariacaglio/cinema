using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Utente
{
    public string Cognome { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Sesso { get; set; } = null!;

    public DateOnly Nascita { get; set; }

    public string Residenza { get; set; } = null!;

    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();

    public virtual ICollection<Valutazione> Valutaziones { get; } = new List<Valutazione>();
}
