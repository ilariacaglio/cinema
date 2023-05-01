using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.Models.VM
{
	public class UtenteVM
	{
		public IdentityUser? user { get; set; }
		public string? ruolo { get; set; }
		public IEnumerable<SelectListItem>? ruoli { get; set; }
	}
}