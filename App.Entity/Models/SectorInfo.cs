using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class SectorInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string SectorId { get; set; }

        [StringLength(100)]
        public string SectorName { get; set; }

        [StringLength(100)]
        public string SectorCode { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }
        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
    }
}