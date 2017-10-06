using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class EmployeeDesignation : BaseModel
    {

        [Required]
        [StringLength(20)]
        [Display(Name = "Designation Id")]
        public string DesignationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Designation Name (EN)")]
        public string DesignationTitleEn { get; set; }

        [StringLength(100)]
        [Display(Name = "Designation Name (BN)")]
        public string DesignationTitleBn { get; set; }

        [StringLength(20)]
        [Display(Name = "Designation Depertment")]
        public string DesignationDepertment { get; set; }

        public Status Status { get; set; }
    }
}