using System;
namespace Cinema.Models.VM
{
	public class PrenotazioneDetailsVM
	{
			public Prenotazione prenotazione { get; set; }
			public List<Posto> postiPrenotati { get; set; }
	}
}