using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class BankAccount : BaseModel
    {
        public BankAccount()
        {
            Balance = 0;
        }
        [Required]
        [Display(Name = "Account Id")]
        public string AccountId { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Display(Name = "Account Number")]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [StringLength(50)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Range(0,double.MaxValue)]
        public double Balance { get; set; }

        public Status Status { get; set; }
    }
}
