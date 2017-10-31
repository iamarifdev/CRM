using System;
using System.ComponentModel.DataAnnotations;
using App.Entity.Models;

namespace App.Web.Models
{
    public class DepositViewModel
    {
        public DepositViewModel()
        {
            Date = DateTime.Now;
        }

        public int Id { get; set; }

        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Payer Type")]
        public PayerType PayerType { get; set; }

        [Display(Name = "Payer")]
        public int? PayerId { get; set; }
        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}