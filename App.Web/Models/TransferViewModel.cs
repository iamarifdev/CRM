using System;
using System.ComponentModel.DataAnnotations;

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
        public int AccountFrom { get; set; }

        [Display(Name = "To Account")]
        public int AccountTo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public int MethodId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}