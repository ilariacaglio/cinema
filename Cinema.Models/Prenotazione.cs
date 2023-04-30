using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Prenotazione
{
    public int Id { get; set; }

    [Display(Name = "Data")]
    public DateOnly DataS { get; set; }

    [Display(Name = "Ora")]
    public TimeOnly OraS { get; set; }

    [Display(Name = "Sala")]
    public int IdSala { get; set; }

    [Display(Name = "Utente")]
    public string IdUtente { get; set; } = null!;
    
    public bool Pagato { get; set; }

    [ValidateNever]
    public virtual ICollection<Comprende> Comprendes { get; } = new List<Comprende>();

    [ValidateNever]
    public virtual Utente IdUtenteNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Spettacolo Spettacolo { get; set; } = null!;
}
