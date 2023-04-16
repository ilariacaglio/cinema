using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models;

public partial class Utente : IdentityUser
{
    public string Cognome { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Sesso { get; set; } = null!;

    public DateOnly Nascita { get; set; }

    public string Residenza { get; set; } = null!;

    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();

    public virtual ICollection<Valutazione> Valutaziones { get; } = new List<Valutazione>();
}