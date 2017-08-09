using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "Delete Status")]
        public bool DelStatus { get; set; }

        [Display(Name = "EntryDate")]
        public DateTime EntryDate { get; set; }

        [ForeignKey("User")]
        public int EntryBy { get; set; }
        public virtual User User { get; set; }
    }
}
