using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class ClientPaymentReportViewModel
    {
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
    }
}