using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public double PaymentAmount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }

        [StringLength(10)]
        public string Channel { get; set; }

        [StringLength(10)]
        public string Status { get; set; }
    }
}