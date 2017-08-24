using System.ComponentModel.DataAnnotations;
namespace App.Entity.Models
{
    public class AirLineInfo : BaseModel
    {

        [Required]
        [StringLength(20)]
        [Display(Name = "AirLine Id")]
        public string AirLineId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "AirLine Name")]
        public string AirLineName { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public Status Status { get; set; }

    }
}