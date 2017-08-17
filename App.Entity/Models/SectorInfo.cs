using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class SectorInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string SectorId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Sector Name")]
        public string SectorName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Sector Code")]
        public string SectorCode { get; set; }

        public int Status { get; set; }

        [Display(Name = "Delete Status")]
        public bool DelStatus { get; set; }
        public DateTime EntryDate { get; set; }

        [ForeignKey("User")]
        public int EntryBy { get; set; }

        public virtual User User { get; set; }
    }
}