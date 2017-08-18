using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class BranchInfo : BaseModel
    {
        [Required]
        [Index("IX_Branch_Id",1,IsUnique = true)]
        [StringLength(20)]
        [Display(Name = "Branch")]
        public string BranchId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        
        public Status Status { get; set; }
    }
}