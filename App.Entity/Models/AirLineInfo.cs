using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class AirLineInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string AirLineId { get; set; }

        [StringLength(100)]
        public string AirLineName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }
        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
    }
}