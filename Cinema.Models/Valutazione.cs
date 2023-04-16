using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Valutazione
{
    [Display(Name = "Utente")]
    [Required]
    public string IdUtente { get; set; } = null!;

    [Display(Name = "Film")]
    [Required]
    public int IdFilm { get; set; }

    [Required]
    public double Voto { get; set; }

    [ValidateNever]
    public virtual Film IdFilmNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Utente IdUtenteNavigation { get; set; } = null!;
}
