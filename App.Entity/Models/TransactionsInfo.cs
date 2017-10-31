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

        public int? AccountFrom { get; set; }

        public int? AccountTo { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        public PayerType PayerType { get; set; }

        public int? PayerId { get; set; }

        public decimal Amount { get; set; }

        public int MethodId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey("AccountFrom")]
        public virtual BankAccount BankAccountFrom { get; set; }
        
        [ForeignKey("AccountTo")]
        public virtual BankAccount BankAccountTo { get; set; }
    }
}