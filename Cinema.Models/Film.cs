using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Film
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50)]
    public string Titolo { get; set; } = null!;

    [Required]
    public int Durata { get; set; }

    [Required]
    public DateOnly Anno { get; set; }

    [Required]
    [MaxLength(500)]
    public string? Descrizione { get; set; }

    [Display(Name = "Immagine")]
    public string? Img { get; set; }

    [Required]
    [Display(Name = "Genere")]
    public int IdGenere { get; set; }

    [ValidateNever]
    public virtual Genere IdGenereNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Spettacolo> Spettacolos { get; set; } = new List<Spettacolo>();

    [ValidateNever]
    public virtual ICollection<Valutazione> Valutaziones { get; set; } = new List<Valutazione>();
}
