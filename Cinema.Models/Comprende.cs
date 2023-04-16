using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Comprende
{
    [Required]
    public int IdPosto { get; set; }

    [Required]
    public int IdPrenotazione { get; set; }

    [ValidateNever]
    public virtual Posto Id { get; set; } = null!;

    [ValidateNever]
    public virtual Prenotazione Prenotazione { get; set; } = null!;
}