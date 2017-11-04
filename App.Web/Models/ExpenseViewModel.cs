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

        [System.Web.Mvc.Remote("IsExpenseBalanceAvailable", "Accounts", AdditionalFields = "AccountId", HttpMethod = "POST", ErrorMessage = "Insufficient Balance, check account balance.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}