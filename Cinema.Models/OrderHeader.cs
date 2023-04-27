using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string UtenteId { get; set; } = null!;
        [ForeignKey(nameof(UtenteId))]
        [ValidateNever]
        public Utente Utente { get; set; } = null!;
        public DateTime DataOrdine { get; set; }
        public double TotaleOrdine { get; set; }
        public string? StatoOrdine { get; set; }
        public string? StatoPagamento { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [MinLength(5, ErrorMessage = "{0} must be at least {1} digits")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [DisplayName("Street Address")]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} chars")]
        public string StreetAddress { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} chars")]
        public string City { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} chars")]
        public string State { get; set; } = null!;
        [Required]
        [DisplayName("Postal Code")]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} chars")]
        public string PostalCode { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
    }
}