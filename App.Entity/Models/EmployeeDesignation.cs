using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class EmployeeDesignation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Designation Id")]
        public string DesignationId { get; set; }

        [StringLength(100)]
        [Display(Name = "Designation Name (EN)")]
        public string DesignationTitleEn { get; set; }

        [StringLength(100)]
        [Display(Name = "Designation Name (BN)")]
        public string DesignationTitleBn { get; set; }

        [StringLength(20)]
        public string DesignationDepertment { get; set; }

        public int Status { get; set; }

        public bool DelStatus { get; set; }

        public DateTime EntryDate { get; set; }

        [ForeignKey("User")]
        public int EntryBy { get; set; }

        public virtual User User { get; set; }
    }
}