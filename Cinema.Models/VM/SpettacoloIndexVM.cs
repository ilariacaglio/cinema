using System;
namespace Cinema.Models.VM
{
	public class SpettacoloIndexVM
	{
        public DateOnly Data { get; set; }
        public TimeOnly Ora { get; set; }
		public string? Titolo { get; set; }
		public int salaId { get; set; }
	}
}

