using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace App.Web.Models
{
    public class ClientPaymentViewModel
    {
        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Service Amt")]
        public double ServiceAmount { get; set; }

        [Required]
        [Display(Name = "Paid Amt")]
        public double PaidAmount { get; set; }

        [Required]
        [Display(Name = "Due Amt")]
        public double DueAmount { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        [LessThan("ServiceAmount", ErrorMessage = "Payment amount cannot be greater than service charge.")]
        public double PaymentAmount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }

        [StringLength(10)]
        public string Channel { get; set; }

        [StringLength(10)]
        public string Status { get; set; }
    }
}