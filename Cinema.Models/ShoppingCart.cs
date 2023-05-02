using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cinema.Models
{
	public class ShoppingCart
	{
        public int Id { get; set; }
        public int PrenotazioneId { get; set; }
        [ForeignKey(nameof(PrenotazioneId))]
        [ValidateNever]
        public Prenotazione prenotazione { get; set; } = null!;
        public string UtenteId { get; set; } = null!;
        [ForeignKey(nameof(UtenteId))]
        [ValidateNever]
        public Utente utente { get; set; } = null!;
        [NotMapped]
        public double Price { get; set; }
        [NotMapped]
        public PrenotazioneDetailsVM dettagliPrenotazione { get; set; } = null!;
    }
}