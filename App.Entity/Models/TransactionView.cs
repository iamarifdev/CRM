using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    [Table("V_TransactionInfo")]
    public class TransactionView
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string AccountFrom { get; set; }
        public string AccountTo { get; set; }
        public DateTime Date { get; set; }
        public string Payer { get; set; }
        public string Method { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal TransferAmount { get; set; }
        public string Description { get; set; }
    }
}
