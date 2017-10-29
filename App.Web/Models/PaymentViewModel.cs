using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class PaymentViewModel
    {
        [Display(Name = "Paymnent Made By")]
        public string PaymnentMadeBy { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Payment Amount")]
        public double PaymentAmount { get; set; }
    }
}