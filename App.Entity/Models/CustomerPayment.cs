using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class CustomerPayment : BaseModel
    {
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        public double PaymentAmount { get; set; }

        [Display(Name = "Payment Method")]
        public int? MethodId { get; set; }

        [StringLength(10)]
        public string Channel { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public UserType UserType { get; set; }

        [ForeignKey("MethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ClientInfo ClientInfo { get; set; }

    }
}