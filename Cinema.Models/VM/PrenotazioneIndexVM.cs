using System;
namespace Cinema.Models.VM
{
	public class PrenotazioneIndexVM
	{
        public int id { get; set; }
        public DateOnly dataS { get; set; }
        public TimeOnly oraS { get; set; }
        public int idSala { get; set; }
        public bool pagato { get; set; }
        public string? emailUtente { get; set; }
        public string? titoloFilm { get; set; }
        public double prezzoTot { get; set; }
    }
}

