using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models.VM
{
	public class PrenotazioneVM
	{
		public Prenotazione p { get; set; }
		public int postiSala { get; set; }
		public int fileSala { get; set; }
		[ValidateNever]
		public List<Posto> Prenotati { get; set; } = new List<Posto>();
        [ValidateNever]
        public List<string> Selezionati { get; set; } = new List<string>();
    }
}