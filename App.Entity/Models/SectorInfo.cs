using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class SectorInfo : BaseModel
    {

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

        public Status Status { get; set; }

    }
}