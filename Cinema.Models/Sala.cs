using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cinema.Models;

public partial class Sala
{
    public int Id { get; set; }

    [Display(Name = "Numero Posti")]
    [Required]
    [Range(1, 1000)]
    public int Nposti { get; set; }

    [Display(Name = "Numero File")]
    [Required]
    [Range(1, 1000)]
    public int Nfile { get; set; }

    [Required]
    public bool Isense { get; set; }

    public virtual ICollection<Posto> Postos { get; } = new List<Posto>();

    public virtual ICollection<Spettacolo> Spettacolos { get; } = new List<Spettacolo>();
}
