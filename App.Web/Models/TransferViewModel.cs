using System;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace App.Web.Models
{
    public class TransferViewModel
    {
        public TransferViewModel()
        {
            Date = DateTime.Now;
        }
        public int Id { get; set; }

        [Display(Name = "From Account")]
        [NotEqualTo("AccountTo", ErrorMessage = "From Account cannot be same as To Account.")]
        public int AccountFrom { get; set; }

        [Display(Name = "To Account")]
        [NotEqualTo("AccountFrom", ErrorMessage = "To Account cannot be same as From Account.")]
        public int AccountTo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [System.Web.Mvc.Remote("IsTransferBalanceAvailable", "Accounts", AdditionalFields = "AccountFrom", HttpMethod = "POST", ErrorMessage = "Insufficient Balance, check account balance.")]
        [Range(0.01,double.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}