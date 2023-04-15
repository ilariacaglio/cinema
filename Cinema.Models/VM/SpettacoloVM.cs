using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.Models.VM
{
	public class SpettacoloVM
	{
		public Spettacolo spettacolo { get; set; }
		public DateOnly prevData { get; set; }
        public TimeOnly prevOra { get; set; }
        public int? prevSala { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SalaList { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<SelectListItem> FilmList { get; set; } = null!;
    }
}

