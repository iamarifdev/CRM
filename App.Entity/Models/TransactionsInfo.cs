using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class TransactionsInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionId { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [ForeignKey("BankAccountFrom")]
        [Display(Name = "From Account")]
        public int? AccountFrom { get; set; }

        [ForeignKey("BankAccountTo")]
        [Display(Name = "To Account")]
        public int? AccountTo { get; set; }

        public DateTime? Date { get; set; }

        public PayerType? PayerType { get; set; }

        public int? PayerId { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey("PaymentMethod")]
        public int MethodId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public virtual BankAccount BankAccountFrom { get; set; }
        public virtual BankAccount BankAccountTo { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}