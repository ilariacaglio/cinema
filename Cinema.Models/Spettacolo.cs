using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Spettacolo
{
    public DateOnly Data { get; set; }

    public TimeOnly Ora { get; set; }

    public int IdFilm { get; set; }

    public int IdSala { get; set; }

    [ValidateNever]
    public virtual Film IdFilmNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Sala IdSalaNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();
}
