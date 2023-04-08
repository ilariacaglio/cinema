using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Genere
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Film> Films { get; } = new List<Film>();
}
