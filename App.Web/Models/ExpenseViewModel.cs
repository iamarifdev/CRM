using System;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class ExpenseViewModel
    {
        public ExpenseViewModel()
        {
            Date = DateTime.Now;
        }

        public int Id { get; set; }

        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}