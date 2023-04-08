using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Valutazione
{
    public string IdUtente { get; set; } = null!;

    public int IdFilm { get; set; }

    public double Voto { get; set; }

    public virtual Film IdFilmNavigation { get; set; } = null!;

    public virtual Utente IdUtenteNavigation { get; set; } = null!;
}
