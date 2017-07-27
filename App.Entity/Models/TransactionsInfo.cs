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
        [StringLength(20)]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(10)]
        public string Account { get; set; }

        [StringLength(10)]
        public string AccountTo { get; set; }

        public DateTime Date { get; set; }

        [StringLength(8)]
        public string PayerType { get; set; }

        [StringLength(10)]
        public string PayerId { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [StringLength(10)]
        public string MethodId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Description { get; set; }
    }
}