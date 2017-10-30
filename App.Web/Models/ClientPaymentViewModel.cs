using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using App.Entity.Models;
using Foolproof;

namespace App.Web.Models
{
    public class ClientPaymentViewModel
    {
        public ClientPaymentViewModel()
        {
            PaymentDate = DateTime.Now;
        }

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
        [Required(ErrorMessage = "Select a date for payment.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        [LessThanOrEqualTo("DueAmount", ErrorMessage = "Payment amount cannot be greater than Due amount")]
        public double PaymentAmount { get; set; }

        [Display(Name = "Payment Method")]
        public int? MethodId { get; set; }

        [Required]
        public Channel Channel { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}