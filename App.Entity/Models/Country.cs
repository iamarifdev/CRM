using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class Country
    {
        [Key]
        [Display(Name = "Country Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Display(Name = "Delete Status")]
        public bool DelStatus { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Entry By")]
        public int EntryBy { get; set; }
        public virtual User User { get; set; }
    }
}