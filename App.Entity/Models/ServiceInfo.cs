using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class ServiceInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Service Id")]
        public string ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int Status { get; set; }

        [Display(Name = "Delete Status")]
        public bool DelStatus { get; set; }

        public DateTime EntryDate { get; set; }

        [ForeignKey("User")]
        public int EntryBy { get; set; }
        public virtual User User { get; set; }
    }
}