using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Spettacolo
{
    public DateOnly Data { get; set; }

    public TimeOnly Ora { get; set; }

    public int IdFilm { get; set; }

    public int IdSala { get; set; }

    public virtual Film IdFilmNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();
}
