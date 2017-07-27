using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class CustomerPayment
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string BranchId { get; set; }

        [StringLength(20)]
        public string CustomerId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? PaymentAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string MethodId { get; set; }

        [StringLength(10)]
        public string Channel { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }
    }
}