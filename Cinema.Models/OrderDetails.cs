using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
	public class OrderDetails
	{
        public int Id { get; set; }
        [Required] public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; } = null!;
        [Required] public int PrenotazioneId { get; set; }
        [ForeignKey(nameof(PrenotazioneId))]
        [ValidateNever]
        public Prenotazione Prenotazione { get; set; } = null!;
        public double Price { get; set; }
    }
}