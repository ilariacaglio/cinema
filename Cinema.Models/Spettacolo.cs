using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Spettacolo
{
    [Required]
    public DateOnly Data { get; set; }

    [Required]
    public TimeOnly Ora { get; set; }

    [Required]
    [Display(Name = "Film")]
    public int IdFilm { get; set; }

    [Required]
    [Display(Name = "Sala")]
    public int IdSala { get; set; }

    [ValidateNever]
    public virtual Film IdFilmNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Sala IdSalaNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Prenotazione> Prenotaziones { get; set; } = new List<Prenotazione>();
}
