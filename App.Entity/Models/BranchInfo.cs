using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class BranchInfo
    {
        public int Id { get; set; }

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
        
        [Required]
        public int Status { get; set; }

        public bool DelStatus { get; set; }
        public DateTime? EntryDate { get; set; }

        [ForeignKey("User")]
        public int EntryBy { get; set; }

        public virtual User User { get; set; }
    }
}