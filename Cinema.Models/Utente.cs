using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models;

public partial class Utente 
{
    [PersonalData]
    public string Cognome { get; set; } = null!;

    [PersonalData]
    public string Nome { get; set; } = null!;

    [PersonalData]
    public string Mail { get; set; } = null!;

    [PersonalData]
    public string Password { get; set; } = null!;

    [PersonalData]
    public string Sesso { get; set; } = null!;

    [PersonalData]
    public DateOnly Nascita { get; set; }

    [PersonalData]
    public string Residenza { get; set; } = null!;

    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();

    public virtual ICollection<Valutazione> Valutaziones { get; } = new List<Valutazione>();
}
