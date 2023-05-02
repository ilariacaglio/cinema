using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models;

public partial class Genere
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Caratteri non permessi")]
    public string Nome { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Film> Films { get; } = new List<Film>();
}
