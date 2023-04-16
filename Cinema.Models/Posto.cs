using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Posto
{
    public int Id { get; set; }

    [Required]
    [Range(1, 100)]
    public int Fila { get; set; }

    [Required]
    [Range(1, 250)]
    public int Numero { get; set; }

    [Required]
    public double Costo { get; set; }

    [Required]
    public int IdSala { get; set; }

    [ValidateNever]
    public virtual ICollection<Comprende> Comprendes { get; } = new List<Comprende>();

    [ValidateNever]
    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
