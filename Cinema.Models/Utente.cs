using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Models;

public partial class Utente : IdentityUser
{
    [PersonalData]
    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Caratteri non permessi")]
    public string Cognome { get; set; } = null!;

    [PersonalData]
    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Caratteri non permessi")]
    public string Nome { get; set; } = null!;

    [PersonalData]
    [Required]
    [MaxLength(1)]
    public string Sesso { get; set; } = null!;

    [PersonalData]
    [Required]
    public DateOnly Nascita { get; set; }

    [PersonalData]
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Caratteri non permessi")]
    public string Residenza { get; set; } = null!;

    public virtual ICollection<Prenotazione> Prenotaziones { get; } = new List<Prenotazione>();

    public virtual ICollection<Valutazione> Valutaziones { get; } = new List<Valutazione>();
}