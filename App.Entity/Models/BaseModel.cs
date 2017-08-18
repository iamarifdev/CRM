using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

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
