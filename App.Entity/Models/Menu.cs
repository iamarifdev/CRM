using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        [Display(Name = "Module Name")]
        public Module ModuleName { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(20)]
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(100)]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        public Status Status { get; set; }
    }
}
