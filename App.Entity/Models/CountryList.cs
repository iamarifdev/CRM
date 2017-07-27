using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class CountryList
    {
        public int CountryId { get; set; }

        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }

        public int DelStatus { get; set; }
    }
}